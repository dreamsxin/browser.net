using System;
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
using System.Windows.Shapes;
using Newtonsoft.Json;
using Eotu.Client.Browser;

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
                browser.AddMessageEventListener("ShowMessage", ((string json) => ShowMessage(json)));
                browser.Navigate(EotuCore.Config.domain + "app/index/index");
                browser.WebBrowserFocus.Activate();
            }
            browserInitCompleted = true;
        }

        public void ShowMessage(string json)
        {
            BrowserMessage message = JsonConvert.DeserializeObject<BrowserMessage>(json);
            MessageBox.Show(message.message, message.title);
        }
    }
}
