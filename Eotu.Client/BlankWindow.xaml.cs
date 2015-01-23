﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Eotu.Client.Browser;
using Eotu.Client.Util;
using Newtonsoft.Json;
using System.Net;
using Eotu.Client.Event;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Eotu.Client
{
    /// <summary>
    /// BlankWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BlankWindow : BaseWindow
    {
        private bool browserInitCompleted;
        private string page_json = null;
        public BlankWindow()
        {
            InitializeComponent();
        }

        public BlankWindow(BaseWindow win, string json = null)
            : base(win)
        {
            InitializeComponent();

            page_json = json;

            win.SendMessage += new RoutedEventHandler(win_SendMessage);
        }

        void win_SendMessage(object sender, RoutedEventArgs e)
        {
            MessageEventArgs arg = e as MessageEventArgs;
            if (arg.type == Message.TYPE_CALL_METHOD)
            {
                MethodInfo mm = this.GetType().GetMethod(arg.method);
                object obj = mm.Invoke(this, new object[] { arg.param });
            } else {
                MessageBox.Show(arg.body);
            }
        }

        private void browser_OnInitCompleted(object sender, EventArgs e)
        {
            if (!browserInitCompleted)
            {
                browser.AddMessageEventListener("CreateWindow", ((string json) => CreateWindow(json)));
                browser.AddMessageEventListener("SendWindowMessage", ((string json) => SendWindowMessage(json)));
                browser.AddMessageEventListener("RecvWindowMessage", ((string json) => RecvWindowMessage(json)));
                browser.AddMessageEventListener("AjaxGet", ((string json) => AjaxGet(json)));
                browser.AddMessageEventListener("ShowMessage", ((string json) => ShowMessage(json)));
                browser.AddMessageEventListener("PlaySound", ((string json) => PlaySound(json)));
                browser.AddMessageEventListener("SetWindowActivate", ((string json) => SetWindowActivate(json)));
                browser.AddMessageEventListener("SetWindowTitle", ((string json) => SetWindowTitle(json)));
                browser.AddMessageEventListener("SetWindowStyle", ((string json) => SetWindowStyle(json)));
                browser.AddMessageEventListener("SetResizeMode", ((string json) => SetResizeMode(json)));
                browser.AddMessageEventListener("SetWindowSize", ((string json) => SetWindowSize(json)));
                browser.AddMessageEventListener("Navigate", ((string json) => Navigate(json)));
                browser.WebBrowserFocus.Activate();

                if (page_json != null) {
                    Navigate(page_json);
                }

                JObject data = new JObject();
                data["type"] = Message.TYPE_INITCOMPLETED;
                OnSendMessage(MessageEventArgs.Create(SendMessageEvent, data.ToString()));
            }
            browserInitCompleted = true;
        }

        public void CreateWindow(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);

            BlankWindow window = new BlankWindow(this);
            window.Show();
        }

        public void ShowMessage(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            MessageBox.Show(message.message, message.title);
        }

        public void PlaySound(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            string path = message.path;
            if (message.local)
            {
                path = Path.Combine(ExecutionEnvironment.DirectoryOfExecutingAssembly, path);
                path = Path.GetFullPath(path);
            }
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(path);
            player.Play();
        }

        public void SetWindowActivate(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            this.Topmost = message.topmost;
            this.Activate();
        }

        public void SetWindowTitle(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            this.Title = message.title;
        }

        public void SetWindowStyle(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            switch (message.style)
            {
                case "None":
                    this.WindowStyle = WindowStyle.None;
                    break;
                case "SingleBorderWindow":
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    break;
                case "ThreeDBorderWindow":
                    this.WindowStyle = WindowStyle.ThreeDBorderWindow;
                    break;
                case "ToolWindow":
                    this.WindowStyle = WindowStyle.ToolWindow;
                    break;
            }
        }

        public void SetResizeMode(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            switch (message.mode)
            {
                case "CanMinimize":
                    this.ResizeMode = ResizeMode.CanMinimize;
                    break;
                case "CanResize":
                    this.ResizeMode = ResizeMode.CanResize;
                    break;
                case "CanResizeWithGrip":
                    this.ResizeMode = ResizeMode.CanResizeWithGrip;
                    break;
            }
        }

        public void SetWindowSize(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            browser.Width = message.width;
            browser.Height = message.height;
        }

        public void Navigate(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);

            string url = message.url;
            if (message.local)
            {
                url = Path.Combine(ExecutionEnvironment.DirectoryOfExecutingAssembly, url);
                url = Path.GetFullPath(url);
            }
            var uri = new Uri(url);
            browser.Navigate(uri.AbsoluteUri);
            browser.WebBrowserFocus.Activate();
        }

        public void AjaxGet(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(message.url);
            request.Method = "GET";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();

            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string data = readStream.ReadToEnd().Replace("'", "\\'");
            JSCall("Eotu.Success('" + data + "');");
        }

        public string JSCall(string script)
        {
            string outString = "";
            using (var js = new Gecko.AutoJSContext(browser.Window.JSContext))
            {
                try
                {
                    js.EvaluateScript(script, (Gecko.nsISupports)browser.Window.DomWindow, out outString);
                }
                catch (Gecko.GeckoJavaScriptException ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
                catch (System.NotImplementedException ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
            return outString;
        }
    }
}
