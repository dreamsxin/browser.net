using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstFloor.ModernUI.Presentation;

namespace Eotu.Client.Content
{
    class ContactsViewModel
        : NotifyPropertyChanged
    {
        private List<GroupsModel> groups = new List<GroupsModel>();

        public List<GroupsModel> Groups
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
            GroupsModel group1 = new GroupsModel();
            group1.GroupId = 1;
            group1.GroupName = "好友";
            Groups.Add(group1);

            GroupsModel group2 = new GroupsModel();
            group2.GroupId = 2;
            group2.GroupName = "家人";
            groups.Add(group2);

            foreach (GroupsModel group in groups)
            {
                this.grouplinks.Add(new Link { DisplayName = group.GroupName, Source = new Uri(group.GroupId.ToString(), UriKind.Relative) });
            }
        }
    }
}
