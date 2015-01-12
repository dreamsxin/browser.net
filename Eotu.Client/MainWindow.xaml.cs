using FirstFloor.ModernUI.Windows.Controls;
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
using FirstFloor.ModernUI.Presentation;

namespace Eotu.Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            // 显示
            new SplashWindow().ShowDialog();

            helpLink.Source = new Uri(EotuCore.Config.domain);
            loadAppLinkGroup();
        }

        private void loadAppLinkGroup()
        {
            appLinkGroup.Links.Add(new Link { DisplayName = "旅游", Source = new Uri("/Pages/WebBrowser.xaml#app/travel/index/index", UriKind.Relative) });
            appLinkGroup.Links.Add(new Link { DisplayName = "教育", Source = new Uri("/Pages/WebBrowser.xaml#app/edu/index/index", UriKind.Relative) });
        }
    }
}
