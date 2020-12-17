using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using AutoService.Models;
using AutoService.Tools;
using AutoService.Tools.Managers;
using AutoService.Tools.MVVM;
using AutoService.Tools.Navigation;

namespace AutoService.ViewModels
{
    class CarCatalogViewModel : BaseViewModel
    {
        #region Fields

        public ObservableCollection<Car> _carsCollection;
        private ObservableCollection<string> _marks;
        private string _selectedmark;
        private ObservableCollection<string> _models;
        private string _selectedmodel;
        private ObservableCollection<string> _gearBox;
        private string _selectedGearBox;
        private bool _modelEnabled;
        private string _leftRange;
        private string _rightRange;

        private int _leftVal;
        private int _rigthval;

        #region Commands
        private RelayCommand<object> _userProfileCommand;
        private RelayCommand<object> _searchCommand;
        #endregion
        #endregion

        internal CarCatalogViewModel()
        {
            _carsCollection = new ObservableCollection<Car>();
            _models = new ObservableCollection<string>();
            _models.Add("Not Selected");
            _marks = new ObservableCollection<string>();
            _marks.Add("Not Selected");
            SelectedMark = _marks[0];
            SelectedModel = _models[0];
            _gearBox = new ObservableCollection<string>();
            _gearBox.Add("Not Selected");
            SelectedGearBox = _gearBox[0];
            _modelEnabled = false;
            LeftVal = (int)StationManager.getMinPrice();
            LeftRange = LeftVal.ToString();
            RightVal = (int)StationManager.getMaxPrice();
            RightRange = RightVal.ToString();
        }

        public ObservableCollection<Car> CarsCollection
        {
            get { return _carsCollection; }
            private set
            {
                _carsCollection = value;
                OnPropertyChanged();
            }
        }

        #region Commands
        public RelayCommand<object> UserProfileCommand
        {
            get
            {
                return _userProfileCommand ?? (_userProfileCommand = new RelayCommand<object>(
                           UserProfileImplementation));
            }
        }

        public RelayCommand<object> SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new RelayCommand<object>(
                           SearchImplementation));
            }
        }
        #endregion

        private /*async*/ void UserProfileImplementation(object obj)
        {
            /*LoaderManager.Instance.ShowLoader();
            await Task.Run(() => {
                //Thread.Sleep(200);
                // to do login DB
            });
            LoaderManager.Instance.HideLoader();*/
            //MessageBox.Show($"Login successful for user {_login}");
            NavigationManager.Instance.Navigate(ViewType.UserProfile);
        }
        public ObservableCollection<string> MarkList
        {
            get 
            {
                var list = StationManager.getAllMarks();
                list.Insert(0, "Not Selected");
                _marks = new ObservableCollection<string>(list);
                return _marks;
            }
        }

        public ObservableCollection<string> GearBoxList
        {
            get
            {
                var list = StationManager.getAllGearBoxes();
                list.Insert(0, "Not Selected");
                _gearBox = new ObservableCollection<string>(list);
                return _gearBox;
            }
        }

        public decimal MinimumVal
        {
            get { return StationManager.getMinPrice(); }
        }

        public decimal MaximumVal
        {
            get { return StationManager.getMaxPrice(); }
        }

        public string SelectedGearBox
        {
            get
            {
                return _selectedGearBox;
            }
            set
            {
                _selectedGearBox = value;
                OnPropertyChanged();
            }
        }

        public string SelectedMark 
        {
            get
            {
                return _selectedmark;
            }
            set
            {
                _selectedmark = value;
                UpdateModels();
                OnPropertyChanged();
            }
        }

        public string SelectedModel
        {
            get
            {
                return _selectedmodel;
            }
            set
            {
                _selectedmodel = value;
                OnPropertyChanged();
            }
        }

        public bool ModelEnabled
        {
            get
            {
                return _modelEnabled;
            }
            private set
            {
                _modelEnabled = value;
                OnPropertyChanged();
            }
        }
        
        public string LeftRange
        {
            get { return _leftRange; }
            set 
            { 
                _leftRange = value;
                OnPropertyChanged();
            }
        }

        public string RightRange
        {
            get { return _rightRange; }
            set
            {
                _rightRange = value;
                OnPropertyChanged();
            }
        }

        public int RightVal
        {
            get { return _rigthval; }
            set
            {
                _rigthval = value;
                RightRange = _rigthval.ToString();
            }
        }

        public int LeftVal
        {
            get { return _leftVal; }
            set 
            {
                _leftVal = value; 
                LeftRange = _leftVal.ToString();
            }
        }

        void UpdateModels()
        {
            var list = StationManager.getAllModels(SelectedMark);
            ModelEnabled = list.Count == 0 ? false : true;
            list.Insert(0,"Not Selected");
            ModelList = new ObservableCollection<string>(list);
            SelectedModel = list[0];
        }


        public ObservableCollection<string> ModelList
        {
            get
            {
                return _models;
            }
            set
            {
                _models = value;
                OnPropertyChanged();
            }
        }

        private async void SearchImplementation(object obj)
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() => {
                //CarsCollection = new ObservableCollection<Car>(StationManager.getAllCars());
                CarsCollection = new ObservableCollection<Car>(StationManager
                    .getCars(SelectedMark, SelectedModel, SelectedGearBox, LeftVal, RightVal));
            });
            //MessageBox.Show($"SEARCHING finished");
            LoaderManager.Instance.HideLoader();
        }

    }
}
