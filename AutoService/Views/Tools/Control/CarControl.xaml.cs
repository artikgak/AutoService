using System.Windows.Controls;
using AutoService.ViewModels;

namespace AutoService.Views.Tools.Control
{
    /// <summary>
    /// Interaction logic for CarControl.xaml
    /// </summary>
    public partial class CarControl : UserControl
    {
        public CarControl()
        {
            InitializeComponent();
            //DataContext = new CarViewModel();
        }
    }
}
