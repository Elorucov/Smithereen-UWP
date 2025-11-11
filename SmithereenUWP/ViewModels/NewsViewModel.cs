using SmithereenUWP.DataModels;
using SmithereenUWP.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmithereenUWP.ViewModels
{
    public class NewsViewModel : ItemsViewModel<object>
    {
        private readonly ReadOnlyCollection<Entity> _sections = new List<Entity> { 
            new Entity(-1, "Friends"),
            new Entity(-2, "Groups"),
            new Entity(-3, "Comments")
        }.AsReadOnly();
        private Entity _currentSection;

        public ReadOnlyCollection<Entity> Sections => _sections;
        public Entity CurrentSection { get { return _currentSection; } set { _currentSection = value; OnPropertyChanged(); } }

        public NewsViewModel()
        {
            CurrentSection = Sections.First();
        }
    }
}