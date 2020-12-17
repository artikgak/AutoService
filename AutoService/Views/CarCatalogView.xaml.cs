using System.Windows.Controls;
using AutoService.Tools.Navigation;
using AutoService.ViewModels;

namespace AutoService.Views
{
    /// <summary>
    /// Interaction logic for CarCatalogView.xaml
    /// </summary>
    public partial class CarCatalogView : UserControl, INavigatable
    {
        public CarCatalogView()
        {
            InitializeComponent();
            DataContext = new CarCatalogViewModel();
        }
    }
}
