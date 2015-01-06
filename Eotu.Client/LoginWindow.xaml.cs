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
using System.Runtime.InteropServices;

namespace Eotu.Client
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void button_login_Click(object sender, RoutedEventArgs e)
        {
            EotuCore.LoginController loginController = new EotuCore.LoginController();
            if (loginController.login(this.textBox_username.Text, this.passwordBox.Password)) { 
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("账户或密码错误", "登录失败", MessageBoxButton.OK);
            }
        }
    }
}
