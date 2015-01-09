﻿using System;
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
using EotuCore;

namespace Eotu.Client.Content
{
    /// <summary>
    /// ContactsList.xaml 的交互逻辑
    /// </summary>
    public partial class ContactsList : UserControl
    {
        public ContactsList(Uri uri)
        {
            InitializeComponent();

            Logger.Debug(uri.ToString());

            // 绑定数据
            App app = (App)Application.Current;

            ContactsViewModel model = app.getContactsViewModel();
            model.loadContacts();
            this.DataContext = model;
        }
    }
}
