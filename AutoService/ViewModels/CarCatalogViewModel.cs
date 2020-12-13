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
    class CarCatalogViewModel
    {
        #region Fields

        #region Commands
        private RelayCommand<object> _userProfileCommand;
        #endregion
        #endregion


        #region Commands
        public RelayCommand<object> UserProfileCommand
        {
            get
            {
                return _userProfileCommand ?? (_userProfileCommand = new RelayCommand<object>(
                           UserProfileImplementation));
            }
        }
        #endregion

        private async void UserProfileImplementation(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() => {
                Thread.Sleep(200);
                // to do login DB
            });
            LoaderManager.Instance.HideLoader();
            //MessageBox.Show($"Login successful for user {_login}");
            NavigationManager.Instance.Navigate(ViewType.UserProfile);
        }

    }
}
