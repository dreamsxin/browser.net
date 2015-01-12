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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;

namespace Eotu.Client.Pages
{
    /// <summary>
    /// WebBrowser.xaml 的交互逻辑
    /// </summary>
    public partial class WebBrowser : UserControl, IContent
    {
        private bool browserInitCompleted;
        private string nav_uri;
        public WebBrowser()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FirstFloor.ModernUI.Windows.Navigation.FragmentNavigationEventArgs e)
        {
            nav_uri = e.Fragment.ToString();
            browser_open(nav_uri);
        }

        public void OnNavigatedFrom(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(FirstFloor.ModernUI.Windows.Navigation.NavigationEventArgs e)
        {
        }

        public void OnNavigatingFrom(FirstFloor.ModernUI.Windows.Navigation.NavigatingCancelEventArgs e)
        {
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            browser_open("http://www.eotu.com:81/");
        }

        private void browser_OnInitCompleted(object sender, EventArgs e)
        {
            browserInitCompleted = true;
            browser_open(nav_uri);
        }

        private void browser_open(string uri)
        {
            if (browserInitCompleted) {
                browser.Navigate("http://www.eotu.com:81/" + uri);
            }
        }
    }
}
