using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoService.Tools.Managers;
using AutoService.Tools.MVVM;
using AutoService.Tools.Navigation;

namespace AutoService.ViewModels
{
    class UserProfileViewModel
    {

        #region Fields

        public string UserInfo
        {
            get { return StationManager.CurrentUser.ToString(); }
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
        #endregion

        private void BackToCatalogImplementation(object obj)
        {
            NavigationManager.Instance.Navigate(ViewType.CarCatalog);
        }

        private async void LogOutImplementation(object obj)
        {

             LoaderManager.Instance.ShowLoader();
             await Task.Run(() => {
                 StationManager.LogOut();
             });

             LoaderManager.Instance.HideLoader();
             NavigationManager.Instance.Navigate(ViewType.SignIn);
        }

    }
}
