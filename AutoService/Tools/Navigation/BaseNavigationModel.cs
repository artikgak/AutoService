﻿using System.Collections.Generic;

namespace AutoService.Tools.Navigation
{
    internal abstract class BaseNavigationModel : INavigationModel
    {

        private readonly IContentOwner _contentOwner;
        private readonly Dictionary<ViewType, INavigatable> _viewsDictionary;

        protected BaseNavigationModel(IContentOwner contentOwner)
        {
            _contentOwner = contentOwner;
            _viewsDictionary = new Dictionary<ViewType, INavigatable>();
        }

        protected IContentOwner ContentOwner
        {
            get { return _contentOwner; }
        }

        protected Dictionary<ViewType, INavigatable> ViewsDictionary
        {
            get { return _viewsDictionary; }
        }

        public void Navigate(ViewType viewType)
        {
            if (viewType == ViewType.SignIn)
            {
                ViewsDictionary.Remove(ViewType.CarCatalog);
                ViewsDictionary.Remove(ViewType.UserProfile);
            }
            if (viewType == ViewType.CarCatalog)
            {
                ViewsDictionary.Remove(ViewType.UserProfile);
            }
            if (!ViewsDictionary.ContainsKey(viewType))
                InitializeView(viewType);
            ContentOwner.ContentControl.Content = ViewsDictionary[viewType];
        }

        protected abstract void InitializeView(ViewType viewType);

    }

}
