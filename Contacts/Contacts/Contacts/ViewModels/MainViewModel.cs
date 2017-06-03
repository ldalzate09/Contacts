using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.ViewModels
{
    class MainViewModel
    {
        public ContactsViewModel Contacts { get; set; }
        public MainViewModel()
        {
            Contacts = new ContactsViewModel();
        }
    }
}
