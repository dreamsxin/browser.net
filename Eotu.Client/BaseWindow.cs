using System;
using System.ComponentModel;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eotu.Client.Event;

namespace Eotu.Client
{
    public class BaseWindow : Window
    {
        #region Message Event
        public static readonly RoutedEvent SendMessageEvent = EventManager.RegisterRoutedEvent(
        "SendMessage", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BaseWindow));

        [Category("Send Message Events")]
        public event RoutedEventHandler SendMessage
        {
            add { AddHandler(SendMessageEvent, value); }
            remove { RemoveHandler(SendMessageEvent, value); }
        }

        protected virtual void OnSendMessage(MessageEventArgs e)
        {
            RaiseEvent(e);
        }

        public static readonly RoutedEvent RecvMessageEvent = EventManager.RegisterRoutedEvent(
        "RecvMessage", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(BaseWindow));

        [Category("Recv Message Events")]
        public event RoutedEventHandler RecvMessage
        {
            add { AddHandler(RecvMessageEvent, value); }
            remove { RemoveHandler(RecvMessageEvent, value); }
        }

        protected virtual void OnRecvMessage(MessageEventArgs e)
        {
            RaiseEvent(e);
        }
        #endregion

        public BaseWindow mainWindown = null;
        public BaseWindow()
        {
        }

        public BaseWindow(BaseWindow win)
        {
            mainWindown = win;
        }

        public void SendWindowMessage(string json)
        {
            OnSendMessage(MessageEventArgs.Create(SendMessageEvent, json));
        }

        public void RecvWindowMessage(string json)
        {
            OnRecvMessage(MessageEventArgs.Create(RecvMessageEvent, json));
        }
    }
}
