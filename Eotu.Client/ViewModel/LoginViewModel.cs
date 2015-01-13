using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstFloor.ModernUI.Presentation;
using EotuCore;

namespace Eotu.Client.Content
{
    public class LoginViewModel
        : NotifyPropertyChanged
    {
        private String username;
        public String Username
        {
            get
            {
                return this.username;
            }

            set
            {
                if (value != this.username)
                {
                    this.username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        private String password;
        public String Password
        {
            get
            {
                return this.password;
            }

            set
            {
                if (value != this.password)
                {
                    this.password = value;
                    OnPropertyChanged("Password");
                }
            }
        }
    }
}
