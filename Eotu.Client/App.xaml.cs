using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;
using EotuCore;
using Eotu.Client.Content;

namespace Eotu.Client
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private NotifyIcon notifyIcon;
        private ContactsViewModel contactsViewModel;

		/// <summary>  
		/// 只打开一个进程  
		/// </summary>  
		/// <param name="e"></param>  
		protected override void OnStartup(StartupEventArgs e)
		{
			Process currentProcess = Process.GetCurrentProcess();  
			int count = Process.GetProcessesByName(currentProcess.ProcessName).Count();

			if (count > 1) {
				MessageBox.Show("Already an instance is running...");
				App.Current.Shutdown(); 
			} else {
				// base.OnStartup(e);
                RemoveTrayIcon();
                AddTrayIcon();
            }
		}

		protected override void OnExit(ExitEventArgs e)
		{
			RemoveTrayIcon();
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

        public ContactsViewModel getContactsViewModel()
        {
            if (contactsViewModel == null) {
                contactsViewModel = new ContactsViewModel();
            }
            return contactsViewModel;
        }
    }
}
