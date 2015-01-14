using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using EotuCore;
using Eotu.Client.Content;
using Eotu.Client.Browser;
using Gecko;

namespace Eotu.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        private NotifyIcon notifyIcon;
        private static ContactsViewModel contactsViewModel;

		/// <summary>  
		/// 只打开一个进程  
		/// </summary>  
		/// <param name="e"></param>  
		protected override void OnStartup(StartupEventArgs e)
		{
			Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            int count = processes.Count();

			if (count > 1) {
                if (IsIconic(processes[0].MainWindowHandle))
                {
                    ShowWindowAsync(processes[0].MainWindowHandle, 9);
                }
                SetForegroundWindow(processes[0].MainWindowHandle);
				MessageBox.Show("Already an instance is running...");
				App.Current.Shutdown(); 
			} else {
				// base.OnStartup(e);
                RemoveTrayIcon();
                AddTrayIcon();
            }

            loadConfig();
            Xpcom.Initialize(XULRunnerLocator.GetXULRunnerLocation());
            Gecko.PromptFactory.PromptServiceCreator = () => new MyCustomPromptService();
		}

		protected override void OnExit(ExitEventArgs e)
		{
			RemoveTrayIcon();
            Xpcom.Shutdown();
		}

		private void AddTrayIcon()
		{
			if (notifyIcon != null) {
				return;
			}
			notifyIcon = new NotifyIcon{
                Icon = Eotu.Client.Properties.Resources.notify,
				Text = "优途"
			};
			notifyIcon.Visible = true;

			ContextMenu contextMenu = new ContextMenu();

			MenuItem openItem = new MenuItem();
			openItem.Text = "打开";

			MenuItem closeItem = new MenuItem();
			closeItem.Text = "退出";
			closeItem.Click += new EventHandler(delegate { this.Shutdown(); });

			contextMenu.MenuItems.Add(openItem);
			contextMenu.MenuItems.Add(closeItem);

            notifyIcon.ContextMenu = contextMenu;

            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler((o, e) =>
            {
                Logger.Debug("双击");
            });
		}

		private void RemoveTrayIcon()
		{
			if (notifyIcon != null) {
				notifyIcon.Visible = false;
				notifyIcon.Dispose();
				notifyIcon = null;
			}
		}

        /// <summary>
        /// 读取配置文件
        /// </summary>
        private void loadConfig()
        {
            EotuCore.Config.domain = ConfigurationManager.AppSettings["domain"];
            EotuCore.Config.host = ConfigurationManager.AppSettings["host"];
            EotuCore.Config.port = int.Parse(ConfigurationManager.AppSettings["port"]);
            EotuCore.Config.username = ConfigurationManager.AppSettings["username"];
            EotuCore.Config.password = ConfigurationManager.AppSettings["password"];
            EotuCore.Config.token = ConfigurationManager.AppSettings["token"];
        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        public void saveConfig(Dictionary<string, string> values)
        {
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            foreach (var item in values) {
                if (ConfigurationManager.AppSettings[item.Key] == null)
                {
                    configuration.AppSettings.Settings.Add(item.Key, item.Value);
                }
                else
                {
                    configuration.AppSettings.Settings[item.Key].Value = item.Value;
                }

            }
            configuration.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        public ContactsViewModel getContactsViewModel()
        {
            if (contactsViewModel == null) {
                contactsViewModel = new ContactsViewModel();
            }
            return contactsViewModel;
        }
    }
}
