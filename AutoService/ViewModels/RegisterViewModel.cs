using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Tools;
using AutoService.Tools.MVVM;

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
        #endregion



    }
}
