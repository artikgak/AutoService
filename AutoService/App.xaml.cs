using System.Windows;
using AutoService.Tools.Managers;

namespace AutoService
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            StationManager.DataStorage.CloseConnection();
        }
    }
}
