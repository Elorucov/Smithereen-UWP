using System.Collections.ObjectModel;

namespace SmithereenUWP.ViewModels.Base
{
    public class ItemsViewModel<T> : CommonViewModel
    {
        private ObservableCollection<T> _items = new ObservableCollection<T>();

        public ObservableCollection<T> Items { get { return _items; } set { _items = value; OnPropertyChanged(); } }
    }
}
