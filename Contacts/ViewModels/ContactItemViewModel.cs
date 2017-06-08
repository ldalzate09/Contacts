using System;
using System.Windows.Input;
using Contacts.Models;
using Contacts.Services;
using GalaSoft.MvvmLight.Command;

namespace Contacts.ViewModels
{
    public class ContactItemViewModel : Contact
    {
        #region Attributes
        private NavigationService navigationService;
        #endregion

        #region Contructors
        public ContactItemViewModel()
        {
            navigationService = new NavigationService();
        }
        #endregion

        #region Commands
        public ICommand EditContactCommand
        {
            get { return new RelayCommand(EditContact); }
        }

        private async void EditContact()
        {
            var maninViewModel = MainViewModel.GetInstance();
            maninViewModel.EditContact = new EditContactViewModel(this);
            await navigationService.Navigate("EditContactPage");
        }
        #endregion
    }
}