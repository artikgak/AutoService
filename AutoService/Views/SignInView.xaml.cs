using System.Windows.Controls;
using AutoService.ViewModels;

namespace AutoService.Views
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class SignInView : UserControl
    {
        internal SignInView()
        {
            InitializeComponent();
            DataContext = new SignInViewModel();
        }
    }
}
