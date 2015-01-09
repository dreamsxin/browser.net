using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstFloor.ModernUI.Windows;
using Eotu.Client.Content;
using EotuCore;

namespace Eotu.Client.Loader
{
    class ContactsLoader
        : DefaultContentLoader
    {
        /// <summary>
        /// Loads the content from specified uri.
        /// </summary>
        /// <param name="uri">The content uri</param>
        /// <returns>The loaded content.</returns>
        protected override object LoadContent(Uri uri)
        {
            return new ContactsList(uri);
        }
    }
}
