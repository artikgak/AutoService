namespace AutoService.Tools.Navigation
{

    internal enum ViewType
    {
        SignIn,
        UserProfile,
        CarCatalog
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
