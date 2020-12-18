using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AutoService.Models;
using AutoService.Tools;
using AutoService.Tools.Managers;
using AutoService.Tools.MVVM;

namespace AutoService.ViewModels
{
    internal class CarViewModel : BaseViewModel
    {

        #region Fields
        private Car _car;
        private string _rented;
        private Brush _backColor;

        #region Commands
        private RelayCommand<object> _rentCommand;
        #endregion
        #endregion

        #region Constructors
        internal CarViewModel() 
        {
            _car = new Car();
            TotalPrice = "Total sum: ";
        }

        internal CarViewModel(Car car)
        {
            this._car = car;
            TotalPrice = "Total sum: ";
            _backColor = Brushes.SandyBrown;
            _rented = null;
        }
        #endregion

        #region Properties

        public Car Car 
        { 
            get
            {
            return _car; 
            } 
        }

        public string CarMark
        {
            get { return Car.Mark; }
            set 
            { 
                Car.Mark = value;
                OnPropertyChanged();
            }
        }

        public string CarInfo
        {
            get { return Car.CarInfo; }
            set
            {
                Car.CarInfo = value;
                OnPropertyChanged();
            }
        }

        public string Rented
        {
            get { return _rented; }
            private set
            {
                _rented = value;
                OnPropertyChanged();
            }
        }

        public string Model
        {
            get { return Car.Model; }
            set
            {
                Car.Model = value;
                OnPropertyChanged();
            }
        }

        public string PriceInfo
        {
            get { return Car.PriceInfo; }
        }

        private string _totalPrice;
        private DateTime? _dateTimePicked = DateTime.Now;

        public Uri ImgSource
        {
            get { return Car.ImgSource; }
            set
            {
                Car.ImgSource = value;
                OnPropertyChanged();
            }
        }
        public DateTime TimePicked
        {
            get { return (DateTime)_dateTimePicked; }
            set
            {
                _dateTimePicked = new DateTime(_dateTimePicked.Value.Year, _dateTimePicked.Value.Month, _dateTimePicked.Value.Day,
                    value.Hour, value.Minute, value.Second);
                ReculcTotalPrice();
                OnPropertyChanged();
            }
        }

        public DateTime DatePicked
        {
            get { return (DateTime)_dateTimePicked; }
            set
            {
                _dateTimePicked = new DateTime(value.Year, value.Month, value.Day,
                    _dateTimePicked.Value.Hour, _dateTimePicked.Value.Minute, _dateTimePicked.Value.Second);
                ReculcTotalPrice();
                OnPropertyChanged();
            }
        }

        public double TotalPriceVal { get; set; }

        public string TotalPrice
        {
            get { return _totalPrice; }

            set
            {
                _totalPrice = value;
                OnPropertyChanged();
            }
        }

        public Brush BackColor
        {
            get { return _backColor; }
            set
            { 
                _backColor = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public RelayCommand<object> RentCommand
        {
            get
            {
                return _rentCommand ?? (_rentCommand = new RelayCommand<object>(
                           RentImplementation, o => CanExecuteCommand()));
            }
        }

        private async void RentImplementation(object obj)
        {
            bool rentSucc = false;
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() =>
            {
                if (StationManager.RentAvail())
                {
                    StationManager.AddCarRent(Car.Guid, StationManager.CurrentUser.ID, DateTime.Now, (DateTime)_dateTimePicked);
                    rentSucc = true;
                }
                else rentSucc = false;
            });
            LoaderManager.Instance.HideLoader();
            if (rentSucc)
            {
                MessageBox.Show($"Cheque to {StationManager.CurrentUser.Login}\n***************************\n" +
                    $"Mark: {Car.Mark}\n{Car.CarInfo}" +
                        $"\nRentStart:{DateTime.Now}\nRentEnd:{_dateTimePicked}\n" +
                        $"Price PerHour:{Car.Price}grn\nTotalSum:{(Math.Round(TotalPriceVal, 2)).ToString()}grn" +
                        $"\n***************************\n"
                        , "Rent successfull !!!");
                Rented = "Rented. Have a nice trip";
                BackColor = Brushes.LightGreen;
            } else
                MessageBox.Show($"Dear {StationManager.CurrentUser.Login},\nAccording to our information you " +
                    $"\nare having car rented now.\n\nCompany policy allows only one rent \nat a time for each person.\n\n" +
                    $"If you still have any questions,\nplease contant us: helpCar.Rent@car.rent"
                        , "Rent restricted !!!");
        }
        #endregion

        public void ReculcTotalPrice()
        {
            double priceH = (double)Car.Price / 60;
            double min = (double)_dateTimePicked.Value.Subtract(DateTime.Now).TotalMinutes;
            TotalPriceVal = min * priceH;
            TotalPrice = TotalPriceVal <= 0 ? "Please, select valid time/date" : "Total sum: " + (Math.Round(TotalPriceVal, 2)).ToString() + "grn";
        }

        private bool CanExecuteCommand()
        {
            return TotalPriceVal>0 && BackColor!=Brushes.LightGreen;
        }

    }
}
