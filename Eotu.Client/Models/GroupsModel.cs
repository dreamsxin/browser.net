using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Eotu.Client.Content
{
    class GroupsModel
        : NotifyPropertyChanged
    {

        private int id = 0;
        private string name = String.Empty;

        public int GroupId
        {
            get
            {
                return this.id;
            }

            set
            {
                if (value != this.id)
                {
                    this.id = value;
                    OnPropertyChanged("GroupId");
                }
            }
        }

        public string GroupName
        {
            get
            {
                return this.name;
            }

            set
            {
                if (value != this.name)
                {
                    this.name = value;
                    OnPropertyChanged("GroupName");
                }
            }
        }
    }
}
