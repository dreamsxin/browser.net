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

namespace Eotu.Client.Content
{
    /// <summary>
    /// SettingsAppearance.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsAppearance : UserControl
    {
        public SettingsAppearance()
        {
            InitializeComponent();
            // 绑定数据
            this.DataContext = new SettingsAppearanceViewModel();
        }
    }
}
