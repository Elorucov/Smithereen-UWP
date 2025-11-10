using SmithereenUWP.Core;
using SmithereenUWP.Extensions;
using System;

namespace SmithereenUWP.ViewModels.Base
{
    public class PlaceholderViewModel : BaseViewModel
    {
        private string _header;
        private string _content;
        private string _actionButton;
        private RelayCommand _actionButtonCommand;
        private object _data;

        public string Header { get { return _header; } private set { _header = value; OnPropertyChanged(); } }
        public string Content { get { return _content; } private set { _content = value; OnPropertyChanged(); } }
        public string ActionButton { get { return _actionButton; } private set { _actionButton = value; OnPropertyChanged(); } }
        public RelayCommand ActionButtonCommand { get { return _actionButtonCommand; } private set { _actionButtonCommand = value; OnPropertyChanged(); } }
        public object Data { get { return _data; } private set { _data = value; OnPropertyChanged(); } }

        public static PlaceholderViewModel GetForException(Exception ex, Action action = null)
        {
            var err = ex.ToUnderstandableInfo();
            return new PlaceholderViewModel()
            {
                Header = err.Item1,
                Content = err.Item2,
                ActionButton = "Retry",
                ActionButtonCommand = action != null ? new RelayCommand(o => { action.Invoke(); }) : null,
            };
        }
    }
}
