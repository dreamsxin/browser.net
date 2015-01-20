using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Net;
using Newtonsoft.Json;
using Eotu.Client.Browser;
using Eotu.Client.Util;
using Gecko;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Eotu.Client
{
    /// <summary>
    /// AppWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AppWindow : Window
    {
        private bool browserInitCompleted;
        public AppWindow()
        {
            InitializeComponent();
            this.SourceInitialized += new EventHandler(AppWindow_SourceInitialized);
        }

        void AppWindow_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            HwndSource.FromHwnd(hwnd).AddHook(new HwndSourceHook(WndProc));
        }

        IntPtr WndProc(IntPtr hwnd, int type, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (type)
            {
                case Eotu.Client.App.WM_COPYDATA:
                    App.COPYDATASTRUCT cp = (App.COPYDATASTRUCT)Marshal.PtrToStructure(lParam, typeof(App.COPYDATASTRUCT));
                    MessageBox.Show(cp.lpData);
                    break;
            }
            return IntPtr.Zero;
        }

        private void browser_OnInitCompleted(object sender, EventArgs e)
        {
            if (!browserInitCompleted)
            {
                browser.AddMessageEventListener("AjaxGet", ((string json) => AjaxGet(json)));
                browser.AddMessageEventListener("ShowMessage", ((string json) => ShowMessage(json)));
                browser.AddMessageEventListener("SetWindowActivate", ((string json) => SetWindowActivate(json)));
                browser.AddMessageEventListener("SetWindowTitle", ((string json) => SetWindowTitle(json)));
                browser.AddMessageEventListener("SetWindowStyle", ((string json) => SetWindowStyle(json)));
                browser.AddMessageEventListener("SetResizeMode", ((string json) => SetResizeMode(json)));
                browser.AddMessageEventListener("SetWindowSize", ((string json) => SetWindowSize(json)));
                browser.AddMessageEventListener("Navigate", ((string json) => Navigate(json)));
                string path = Path.Combine(ExecutionEnvironment.DirectoryOfExecutingAssembly, "UI/index.html");
                path = Path.GetFullPath(path);
                var uri = new Uri(path);
                browser.Navigate(uri.AbsoluteUri);
                browser.WebBrowserFocus.Activate();
            }
            browserInitCompleted = true;
        }

        public void ShowMessage(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            MessageBox.Show(message.message, message.title);
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
            browser.Navigate(message.url);
        }

        public void AjaxGet(string json) {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(message.url);
			request.Method = "GET";

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			Stream receiveStream = response.GetResponseStream();

            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string data = readStream.ReadToEnd().Replace("'", "\\'");
            string outString = "";
            using (var js = new Gecko.AutoJSContext(browser.Window.JSContext))
            {
                try
                {
                    js.EvaluateScript("Eotu.Success('" + data + "');", (nsISupports)browser.Window.DomWindow, out outString);
                }
                catch (GeckoJavaScriptException ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
                catch (System.NotImplementedException ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
