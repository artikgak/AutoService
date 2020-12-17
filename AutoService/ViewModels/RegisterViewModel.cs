using System;
using System.Threading.Tasks;
using System.Windows;
using AutoService.Exceptions;
using AutoService.Models;
using AutoService.Tools;
using AutoService.Tools.Managers;
using AutoService.Tools.MVVM;
using AutoService.Tools.Navigation;

namespace AutoService.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {

        #region Fields
        private string _login;
        private string _email;
        private string _password1;
        private string _password2;
        #endregion
        #region Commands
        private RelayCommand<object> _registerCommand;
        private RelayCommand<object> _backToSignInCommand;
        private RelayCommand<object> _closeCommand;

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value.Replace(" ", "Space");
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Password1
        {
            get { return _password1; }
            set
            {
                _password1 = value;
                OnPropertyChanged();
            }
        }

        public string Password2
        {
            get { return _password2; }
            set
            {
                _password2 = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<object> RegisterCommand
        {
            get
            {
                return _registerCommand ?? (_registerCommand = new RelayCommand<object>(
                           RegisterImplementation, o => CanExecuteCommand()));
            }
        }

        public RelayCommand<object> BackToSignInCommand
        {
            get
            {
                return _backToSignInCommand ?? (_backToSignInCommand = new RelayCommand<object>(
                           BackToSignInImplementation));
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
        private bool CanExecuteCommand()
        {
            return !string.IsNullOrWhiteSpace(_login) && !string.IsNullOrWhiteSpace(_email)
                && !string.IsNullOrWhiteSpace(_password1) && !string.IsNullOrWhiteSpace(_password2);
        }

        private async void RegisterImplementation(object obj)
        {
            bool regSuc = false;
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() =>
            {
                if (_login.Length < 5)
                {
                    MessageBox.Show($"Login must be at least 5 symbols long");
                    return;
                }
                if (_login.Length > 10)
                {
                    MessageBox.Show($"Login must be at most 10 symbols long");
                    return;
                }
                if (Email.Length < 5)
                {
                    MessageBox.Show($"Incorrect Email");
                    return;
                }
                if (_password1.Length < 8)
                {
                    MessageBox.Show($"Password must be at least 8 symbols long");
                    return;
                }
                if (!_password1.Equals(_password2))
                {
                    MessageBox.Show($"Passwords do not match");
                    return;
                }

                try
                {
                    string res = EasyEncryption.SHA.ComputeSHA1Hash(_login + _password1 + "secret");
                    res = EasyEncryption.MD5.ComputeMD5Hash(res + "naukma");
                    User us = new User(_login, res, _email);
                    StationManager.Register(us);
                    MessageBox.Show($"Register successful");
                    regSuc = true;
                    // log login successful
                    Login = "";
                    Email = "";
                    Password1 = "";
                    Password2 = "";
                }
                catch (EmailDuplicateException ex)
                {
                    MessageBox.Show(ex.Message);
                    //log
                }
                catch (LoginDuplicateException ex)
                {
                    MessageBox.Show(ex.Message);
                    //log
                }
            });
            LoaderManager.Instance.HideLoader();
            if (regSuc)
                NavigationManager.Instance.Navigate(ViewType.SignIn);
        }

        private async void BackToSignInImplementation(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() =>
            {
                Login = "";
                Email = "";
                Password1 = "";
                Password2 = "";
            });
            LoaderManager.Instance.HideLoader();
            NavigationManager.Instance.Navigate(ViewType.SignIn);
        }


    }
}
