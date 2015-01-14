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
        const int SW_HIDE = 0;
        const int SW_SHOWNORMAL = 1;
        const int SW_SHOWMINIMIZED = 2;
        const int SW_SHOWMAXIMIZED = 3;
        const int SW_SHOWNOACTIVATE = 4;
        const int SW_RESTORE = 9;
        const int SW_SHOWDEFAULT = 10;

        [DllImport("User32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(int hWnd);
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);
        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

        [DllImport("User32.dll")]
        private static extern int RegisterWindowMessage(string lpString);
        //For use with WM_COPYDATA and COPYDATASTRUCT
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
        //For use with WM_COPYDATA and COPYDATASTRUCT
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(int hWnd, int Msg, int wParam, IntPtr lParam);
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(int hWnd, int Msg, int wParam, IntPtr lParam); 
        
        public const int WM_USER = 0x400;
        public const int WM_COPYDATA = 0x4A;

        //Used for WM_COPYDATA for string messages
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        private NotifyIcon notifyIcon;
        private static ContactsViewModel contactsViewModel;

        public int SendWindowsStringMessage(int hWnd, string msg)
        {
            int result = 0;
            if (hWnd > 0)
            {
                byte[] sarr = System.Text.Encoding.Default.GetBytes(msg);
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.lpData = msg;
                cds.cbData = len + 1;
                result = SendMessage(hWnd, WM_COPYDATA, 0, ref cds);
            }
            return result;
        }

		/// <summary>  
		/// 只打开一个进程  
		/// </summary>  
		/// <param name="e"></param>  
		protected override void OnStartup(StartupEventArgs e)
		{
			Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            int count = processes.Count();

            if (count > 1)
            {
                if (IsIconic(processes[0].MainWindowHandle))
                {
                    ShowWindowAsync(processes[0].MainWindowHandle, SW_RESTORE);
                }
                SetForegroundWindow(processes[0].MainWindowHandle);
                MessageBox.Show("Already an instance is running...");

                SendWindowsStringMessage(processes[0].MainWindowHandle.ToInt32(), "dreamsxin@qq.com"); 
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

        public bool bringAppToFront(int hWnd)
        {
            return SetForegroundWindow(hWnd);
        }

        public int sendWindowsStringMessage(int hWnd, int wParam, string msg)
        {
            int result = 0;
            if (hWnd > 0)
            {
                byte[] sarr = System.Text.Encoding.Default.GetBytes(msg);
                int len = sarr.Length;
                COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.lpData = msg;
                cds.cbData = len + 1;
                result = SendMessage(hWnd, WM_COPYDATA, wParam, ref cds);
            }
            return result;
        }

        public int getWindowId(string className, string windowName)
        {
            return FindWindow(className, windowName);
        }
    }
}
