using SmithereenUWP.DataModels;
using SmithereenUWP.Pages.News;
using SmithereenUWP.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.ViewModels
{
    public class SessionViewModel : BaseViewModel
    {
        private readonly ReadOnlyCollection<MainMenuItem> _menuItems = new List<MainMenuItem>
        {
            // new MainMenuItem('', "My profile", null),
            new MainMenuItem('', "My news", typeof(NewsPage)),
            new MainMenuItem('', "My friends", typeof(NewsPage)),
            new MainMenuItem('', "My photos", typeof(NewsPage)),
            new MainMenuItem('', "My messages", typeof(NewsPage)),
            new MainMenuItem('', "My groups", typeof(NewsPage)),
            new MainMenuItem('', "My events", typeof(NewsPage)),
            new MainMenuItem('', "My feedback", typeof(NewsPage)),
            new MainMenuItem('', "My bookmarks", typeof(NewsPage)),
            new MainMenuItem('', "My settings", typeof(NewsPage)),
        }.AsReadOnly();

        private MainMenuItem _selectedMenuItem;

        public ReadOnlyCollection<MainMenuItem> MenuItems => _menuItems;

        public MainMenuItem SelectedMenuItem { get { return _selectedMenuItem; } set { _selectedMenuItem = value; OnPropertyChanged(); } }

        public SessionViewModel()
        {
            SelectedMenuItem = MenuItems.ElementAt(0);
        }
    }
}
