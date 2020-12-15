using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using AutoService.Exceptions;
using AutoService.Tools;
using AutoService.Tools.Managers;
using AutoService.Tools.MVVM;
using AutoService.Tools.Navigation;

namespace AutoService.ViewModels
{
    class SignInViewModel : BaseViewModel
    {

        #region Fields
        private string _login;
        private string _password;

        #region Commands
        private RelayCommand<object> _signInCommand;
        private RelayCommand<object> _closeCommand;
        private RelayCommand<object> _registerCommand;
        #endregion
        #endregion

        #region Properties
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value.Replace(" ", "Space");
                OnPropertyChanged();
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        #region Commands

        public RelayCommand<object> SignInCommand
        {
            get
            {
                return _signInCommand ?? (_signInCommand = new RelayCommand<object>(
                           SignInInplementation, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<object> RegisterCommand
        {
            get
            {
                return _registerCommand ?? (_registerCommand = new RelayCommand<object>(
                           RegisterInplementation));
            }
        }
        

        public RelayCommand<Object> CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new RelayCommand<object>(o => Environment.Exit(0)));
            }
        }

        #endregion
        #endregion

        private bool CanExecuteCommand()
        {
            return !string.IsNullOrWhiteSpace(_login) && !string.IsNullOrWhiteSpace(_password);
        }

        private async void SignInInplementation(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() => {
                try 
                {
                    string res = EasyEncryption.SHA.ComputeSHA1Hash(_login + _password + "secret");
                    res = EasyEncryption.MD5.ComputeMD5Hash(res+"naukma");
                    StationManager.Login(_login, res);
                    // log login successful
                    Login = "";
                    Password = "";
                }
                catch(LoginException)
                {
                    MessageBox.Show($"Invalid login or password");
                    //log
                }
                });
            LoaderManager.Instance.HideLoader();
            if (StationManager.CurrentUser == null) return;
            NavigationManager.Instance.Navigate(ViewType.CarCatalog);
        }

        private async void RegisterInplementation(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() => {
                Login = "";
                Password = "";
            });
            LoaderManager.Instance.HideLoader();
            NavigationManager.Instance.Navigate(ViewType.Register);
        }

    }
}
