using System.Windows;
using System.Windows.Controls;
using AutoService.Tools.Managers;
using AutoService.Tools.Navigation;
using AutoService.ViewModels;

namespace AutoService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IContentOwner
    {
        public ContentControl ContentControl
        {
            get { return _contentControl; }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            NavigationManager.Instance.Initialize(new InitializationNavigationModel(this));
            NavigationManager.Instance.Navigate(ViewType.SignIn);
        }
    }
}
