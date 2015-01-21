using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Eotu.Client.Browser
{
    class MyCustomPromptService
        : Gecko.PromptService
    {
        public override void Alert(string dialogTitle, string text)
        {
            MessageBox.Show(text, dialogTitle);
        }
    }
}
