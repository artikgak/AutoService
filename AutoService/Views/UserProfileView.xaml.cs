using System.Windows.Controls;
using AutoService.Tools.Navigation;
using AutoService.ViewModels;

namespace AutoService.Views
{
    /// <summary>
    /// Interaction logic for UserProfileView.xaml
    /// </summary>
    public partial class UserProfileView : UserControl, INavigatable
    {
        public UserProfileView()
        {
            InitializeComponent();
            DataContext = new UserProfileViewModel();
        }
    }
}
