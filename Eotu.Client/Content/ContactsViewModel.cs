using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstFloor.ModernUI.Presentation;
using EotuCore;

namespace Eotu.Client.Content
{
    class ContactsViewModel
        : NotifyPropertyChanged
    {
        private List<GroupModel> groups = new List<GroupModel>();

        public List<GroupModel> Groups
        {
            get
            {
                return this.groups;
            }

            set
            {
                if (value != this.groups)
                {
                    this.groups = value;
                    OnPropertyChanged("Groups");
                }
            }
        }

        private LinkCollection grouplinks = new LinkCollection();
        public LinkCollection GroupLinks
        {
            get { return this.grouplinks; }
            set
            {
                if (this.grouplinks != value)
                {
                    this.grouplinks = value;
                    OnPropertyChanged("GroupLinks");
                }
            }
        }

        public ContactsViewModel()
        {
            loadGroup();
        }

        private void loadGroup()
        {
            ContactController contactController = new ContactController();

            Groups = contactController.getGroups(9);

            foreach (GroupModel group in Groups)
            {
                this.grouplinks.Add(new Link { DisplayName = group.GroupName, Source = new Uri(group.GroupId.ToString(), UriKind.Relative) });
            }

        }
    }
}
