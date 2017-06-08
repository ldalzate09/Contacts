using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Contacts.Models;
using Contacts.Services;
using GalaSoft.MvvmLight.Command;
//using Plugin.Connectivity;

namespace Contacts.ViewModels
{
    public class ContactsViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        private ApiService apiService;
		private DialogService dialogService;
		private bool isRefreshing;
		#endregion

		#region Properties
		public ObservableCollection<ContactItemViewModel> MyContacts { get; set; }

        public bool IsRefreshing
		{
			set
			{
				if (isRefreshing != value)
				{
					isRefreshing = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsRefreshing"));
				}
			}
			get
			{
				return isRefreshing;
			}
		}
		#endregion

		#region Constructors
		public ContactsViewModel()
        {
            instance = this;
            
			apiService = new ApiService();
			dialogService = new DialogService();

			MyContacts = new ObservableCollection<ContactItemViewModel>();
		}
		#endregion

		#region Singleton
		static ContactsViewModel instance;

		public static ContactsViewModel GetInstance()
		{
			if (instance == null)
			{
				instance = new ContactsViewModel();
			}

			return instance;
		}
		#endregion

		#region Methods
		private async void LoadContacts()
		{
			//if (!CrossConnectivity.Current.IsConnected)
			//{
			//	await dialogService.ShowMessage("Error", "Check you internet connection.");
			//	return;
			//}

			//var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
			//if (!isReachable)
			//{
			//	await dialogService.ShowMessage("Error", "Check you internet connection.");
			//	return;
			//}
			
			var response = await apiService.Get<Contact>(
                "http://contactsxamarintata.azurewebsites.net", 
                "/api", 
                "/Contacts");

			if (!response.IsSuccess)
			{
				await dialogService.ShowMessage("Error", response.Message);
				return;
			}

			ReloadContacts((List<Contact>)response.Result);
		}

		private void ReloadContacts(List<Contact> contacts)
		{
			MyContacts.Clear();
			foreach (var contact in contacts.OrderBy(c => c.FirstName)
                                            .ThenBy(c => c.LastName))
			{
				MyContacts.Add(new ContactItemViewModel
				{
					ContactId = contact.ContactId,
					EmailAddress = contact.EmailAddress,
					FirstName = contact.FirstName,
					Image = contact.Image,
					LastName = contact.LastName,
					PhoneNumber = contact.PhoneNumber,
				});
			}
		}
        #endregion

        #region Commands
        public ICommand RefreshCommand { get { return new RelayCommand(Refresh); } }

		public void Refresh()
		{
            IsRefreshing = true;
            LoadContacts();
            IsRefreshing = false;
		}
		#endregion
	}
}