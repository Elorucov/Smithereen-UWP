using SmithereenUWP.API.Objects.Response;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SmithereenUWP.DataTemplateSelectors
{
    public class NewsfeedItemDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Other { get; set; }
        public DataTemplate Post { get; set; }
        public DataTemplate Photo { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is NewsfeedItem nfi)
            {
                switch (nfi.Type)
                {
                    case NewsfeedItemType.Post: return Post;
                    case NewsfeedItemType.Photo:
                    case NewsfeedItemType.PhotoTag: return Photo;
                }
            }

            return Other;
        }
    }
}
