using System;
using AutoService.Views;

namespace AutoService.Tools.Navigation
{
    internal class InitializationNavigationModel : BaseNavigationModel
    {

        public InitializationNavigationModel(IContentOwner contentOwner) : base(contentOwner)
        {

        }

        protected override void InitializeView(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.SignIn:
                    ViewsDictionary.Add(viewType, new SignInView());
                    break;
                case ViewType.UserProfile:
                    ViewsDictionary.Add(viewType, new UserProfileView());
                    break;
                case ViewType.CarCatalog:
                    ViewsDictionary.Add(viewType, new CarCatalogView());
                    break;
                case ViewType.Register:
                    ViewsDictionary.Add(viewType, new RegisterView());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(viewType), viewType, null);
            }
        }


    }
}
