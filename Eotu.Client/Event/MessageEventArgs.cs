using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Windows;

namespace Eotu.Client.Event
{
    public class MessageEventArgs : RoutedEventArgs
    {
        Message _Message;

        protected MessageEventArgs(RoutedEvent routedEvent, Message msg) :
            base(routedEvent)
		{
            _Message = msg;
		}

        public static MessageEventArgs Create(RoutedEvent routedEvent, Message msg)
        {
            return new MessageEventArgs(routedEvent, msg);
        }

        public static MessageEventArgs Create(RoutedEvent routedEvent, string json)
        {
            Message msg = JsonConvert.DeserializeObject<Message>(json);
            return new MessageEventArgs(routedEvent, msg);
        }

        public uint type
        {
            get { return _Message.type; }
        }

        public uint code
        {
            get { return _Message.code; }
        }

        public string method
        {
            get { return _Message.method; }
        }

        public string param
        {
            get { return _Message.param; }
        }

        public string from
        {
            get { return _Message.from; }
        }

        public string to
        {
            get { return _Message.to; }
        }

        public string body
        {
            get { return _Message.body; }
        }
    }
}
