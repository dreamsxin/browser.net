using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirstFloor.ModernUI.Presentation;
using EotuCore;

namespace Eotu.Client.Content
{
    public class ContactsViewModel
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

        private List<ContactModel> contacts = new List<ContactModel>();

        public List<ContactModel> Contacts
        {
            get
            {
                return this.contacts;
            }

            set
            {
                if (value != this.contacts)
                {
                    this.contacts = value;
                    OnPropertyChanged("Contacts");
                }
            }
        }

        private ContactModel selectedContact;
        public ContactModel SelectedContact
        {
            get { return this.selectedContact; }
            set
            {
                if (this.selectedContact != value)
                {
                    this.selectedContact = value;
                    OnPropertyChanged("SelectedContact");
                }
            }
        }

        public ContactsViewModel()
        {
            loadGroup();
        }

        public void loadGroup()
        {
            ContactController contactController = new ContactController();

            Groups = contactController.getGroups(9);

            foreach (GroupModel group in Groups)
            {
                this.grouplinks.Add(new Link { DisplayName = group.GroupName, Source = new Uri(group.GroupName, UriKind.Relative) });
            }

        }

        public void loadContacts()
        {
            ContactController contactController = new ContactController();

            Contacts = contactController.getContacts(9, 0);
        }
    }
}
