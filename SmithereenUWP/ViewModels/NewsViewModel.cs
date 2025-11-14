using SmithereenUWP.API.Objects.Response;
using SmithereenUWP.DataModels;
using SmithereenUWP.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SmithereenUWP.ViewModels
{
    public class NewsViewModel : ItemsViewModel<NewsfeedItem>
    {
        private readonly ReadOnlyCollection<Entity> _sections = new List<Entity> { 
            new Entity(-1, "Friends"),
            new Entity(-2, "Groups"),
            new Entity(-3, "Comments")
        }.AsReadOnly();

        private Entity _currentSection;


        private string _startFrom = null;


        public ReadOnlyCollection<Entity> Sections => _sections;

        public Entity CurrentSection { get { return _currentSection; } set { _currentSection = value; OnPropertyChanged(); } }

        public NewsViewModel()
        {
            CurrentSection = Sections.First();
        }

        public async Task LoadNewsAsync() {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                List<string> filters = new List<string>() { "post", "photo", "photo_tag", "friend", "group", "event", "board", "relation" };

                var response = await SessionViewModel.Current.API.Newsfeed.GetAsync(filters, 25, _startFrom);
                _startFrom = response.NextFrom;

                foreach (var item in response.Items)
                {
                    Items.Add(item);
                }
            } catch (Exception ex) { 
                PlaceholderViewModel.GetForException(ex, async () => await LoadNewsAsync());
            } finally
            {
                IsLoading = false;
            }
        }
    }
}