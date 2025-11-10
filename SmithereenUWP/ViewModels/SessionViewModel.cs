using SmithereenUWP.DataModels;
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
        private readonly IReadOnlyCollection<MainMenuItem> _menuItems = new List<MainMenuItem>
        {
            new MainMenuItem('', "My profile", null),
            new MainMenuItem('', "My friends", null),
            new MainMenuItem('', "My photos", null),
            new MainMenuItem('', "My messages", null),
            new MainMenuItem('', "My groups", null),
            new MainMenuItem('', "My events", null),
            new MainMenuItem('', "My news", null),
            new MainMenuItem('', "My feedback", null),
            new MainMenuItem('', "My bookmarks", null),
            new MainMenuItem('', "My settings", null),
        }.AsReadOnly();

        public IReadOnlyCollection<MainMenuItem> MenuItems => _menuItems;
    }
}
