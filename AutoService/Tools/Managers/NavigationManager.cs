using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoService.Tools.Navigation;
using AutoService.Views;

namespace AutoService.Tools.Managers
{
    class NavigationManager
    {

        internal enum View { SignIn = 0, SignUp = 1, Main = 2 }
        private static readonly object Locker = new object();
        private static NavigationManager _instance;
        private MainWindow _navigationOwner;

        internal void Initialize(MainWindow navigation)
        {
            _navigationOwner = navigation;
        }

        internal static NavigationManager Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;
                lock (Locker)
                {
                    return _instance ?? (_instance = new NavigationManager());
                }
            }
        }

        private INavigationModel _navigationModel;

        private NavigationManager()
        {

        }

        internal void Initialize(INavigationModel navigationModel)
        {
            _navigationModel = navigationModel;
        }

        internal void Navigate(ViewType viewType)
        {
            _navigationModel.Navigate(viewType);
        }

    }

}

