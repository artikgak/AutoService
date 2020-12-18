using System;
using System.Threading.Tasks;
using AutoService.Tools;
using AutoService.Tools.Managers;
using AutoService.Tools.MVVM;
using AutoService.Tools.Navigation;

namespace AutoService.ViewModels
{
    class UserProfileViewModel : BaseViewModel
    {

        #region Fields Properties

        private string _carRentedInfo;

        public string UserInfo
        {
            get { return StationManager.CurrentUser.ToString(); }
        }

        public string CarRentedInfo
        {
            get 
            {
                _carRentedInfo = StationManager.CarRented() != null ? "Present car in rent:\n" + StationManager.CarRented().ToString()
                    : "No car rented at present.";
                return _carRentedInfo;
            }
            set
            {
                _carRentedInfo = value;
                OnPropertyChanged();
            }
        }

        public Uri PhotoImg
        {
            get
            {
                return StationManager.CarRented() != null
                    ? StationManager.CarRented().ImgSource
                    : null;
            }
        }

        #region Commands
        private RelayCommand<object> _backToCatalogCommand;
        private RelayCommand<object> _logOutCommand;
        #endregion
        #endregion

        #region Commands
        public RelayCommand<object> BackToCatalogCommand
        {
            get
            {
                return _backToCatalogCommand ?? (_backToCatalogCommand = new RelayCommand<object>(
                           BackToCatalogImplementation));
            }
        }

        public RelayCommand<object> LogOutCommand
        {
            get
            {
                return _logOutCommand ?? (_logOutCommand = new RelayCommand<object>(
                           LogOutImplementation));
            }
        }
        

        private void BackToCatalogImplementation(object obj)
        {
            NavigationManager.Instance.Navigate(ViewType.CarCatalog);
        }

        private async void LogOutImplementation(object obj)
        {

            LoaderManager.Instance.ShowLoader();
            await Task.Run(() =>
            {
                StationManager.LogOut();
            });

            LoaderManager.Instance.HideLoader();
            NavigationManager.Instance.Navigate(ViewType.SignIn);
        }
        #endregion
    }
}
