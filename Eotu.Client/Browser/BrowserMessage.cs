using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eotu.Client.Browser
{
    class BrowserMessage
    {
        public string type = null;
        public string title = null;
        public string style = null;
        public string mode = null;
        public string message = null;
        public int width = -1;
        public int height = -1;
        public bool local = false;
        public string url = null;
        public string path = null;
        public string data = null;
        public bool topmost = false;
    }
}
