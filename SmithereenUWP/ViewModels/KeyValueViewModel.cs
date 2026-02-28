using SmithereenUWP.ViewModels.Base;

namespace SmithereenUWP.ViewModels
{
    internal class KeyValueViewModel : BaseViewModel
    {
        private string _key;
        private string _value;

        public string Key { get { return _key; } set { _key = value; OnPropertyChanged(); } }
        public string Value { get { return _value; } set { _value = value; OnPropertyChanged(); } }

        public KeyValueViewModel(string key, string value)
        {
            _key = key;
            _value = value;
        }

        public KeyValueViewModel() : this(string.Empty, string.Empty) { }
    }
}
