using System.Windows.Controls;
using AutoService.Tools.Navigation;
using AutoService.ViewModels;

namespace AutoService.Views
{
    /// <summary>
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : UserControl, INavigatable
    {
        public RegisterView()
        {
            InitializeComponent();
            DataContext = new RegisterViewModel();
        }
    }
}
