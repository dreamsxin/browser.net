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
        }

        private void browser_OnInitCompleted(object sender, EventArgs e)
        {
            if (!browserInitCompleted)
            {
                browser.AddMessageEventListener("AjaxGet", ((string json) => AjaxGet(json)));
                browser.AddMessageEventListener("ShowMessage", ((string json) => ShowMessage(json)));
                browser.AddMessageEventListener("SetWindowTitle", ((string json) => SetWindowTitle(json)));
                browser.AddMessageEventListener("SetWindowSize", ((string json) => SetWindowSize(json)));
                string path = Path.Combine(ExecutionEnvironment.DirectoryOfExecutingAssembly, "UI/index.html");
                path = Path.GetFullPath(path);
                var uri = new Uri(path);
                browser.Navigate(uri.AbsoluteUri);
                //browser.Navigate(EotuCore.Config.domain + "app/index/index");
                browser.WebBrowserFocus.Activate();
            }
            browserInitCompleted = true;
        }

        public void ShowMessage(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            MessageBox.Show(message.message, message.title);
        }

        public void SetWindowTitle(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            this.Title = message.title;
        }

        public void SetWindowSize(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            this.Width = message.width;
            this.Height = message.height;
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
