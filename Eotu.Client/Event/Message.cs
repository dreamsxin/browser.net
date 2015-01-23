using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eotu.Client.Event
{
    public class Message
    {
        public static readonly uint TYPE_CALL_METHOD = 1;
        public static readonly uint TYPE_INITCOMPLETED = 2;
        public uint type;
        public uint code;
        public string method;
        public string param;
        public string from;
        public string to;
        public string body;
    }
}
