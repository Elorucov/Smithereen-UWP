using SmithereenUWP.API;
using SmithereenUWP.Core;
using SmithereenUWP.DataModels;
using SmithereenUWP.Pages.News;
using SmithereenUWP.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Core;

namespace SmithereenUWP.ViewModels
{
    public class SessionViewModel : BaseViewModel
    {
        private readonly ReadOnlyCollection<MainMenuItem> _menuItems = new List<MainMenuItem>
        {
            new MainMenuItem('', "My profile", typeof(NewsPage)),
            new MainMenuItem('', "My friends", typeof(NewsPage)),
            new MainMenuItem('', "My photos", typeof(NewsPage)),
            new MainMenuItem('', "My messages", typeof(NewsPage)),
            new MainMenuItem('', "My groups", typeof(NewsPage)),
            new MainMenuItem('', "My events", typeof(NewsPage)),
            new MainMenuItem('', "My news", typeof(NewsPage)),
            new MainMenuItem('', "My feedback", typeof(NewsPage)),
            new MainMenuItem('', "My bookmarks", typeof(NewsPage)),
            new MainMenuItem('', "My settings", typeof(NewsPage)),
        }.AsReadOnly();
        private readonly SmithereenAPI _api;

        private MainMenuItem _selectedMenuItem;

        public ReadOnlyCollection<MainMenuItem> MenuItems => _menuItems;
        public SmithereenAPI API => _api;

        public MainMenuItem SelectedMenuItem { get { return _selectedMenuItem; } set { _selectedMenuItem = value; OnPropertyChanged(); } }

        public static SessionViewModel Current => CoreApplication.Properties["svm"] as SessionViewModel;

        public SessionViewModel()
        {
            _api = new SmithereenAPI(AppParameters.CurrentServer, AppInfo.UserAgent)
            {
                AccessToken = AppParameters.CurrentUserAccessToken
            };
            SelectedMenuItem = MenuItems.ElementAt(6);
        }

        public void SetAsCurrent()
        {
            CoreApplication.Properties["svm"] = this;
        }
    }
}
