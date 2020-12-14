namespace AutoService.Tools.Navigation
{

    internal enum ViewType
    {
        SignIn,
        Register,
        UserProfile,
        CarCatalog
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
