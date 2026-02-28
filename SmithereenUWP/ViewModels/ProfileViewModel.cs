using SmithereenUWP.API.Objects.Main;
using SmithereenUWP.Core;
using SmithereenUWP.ViewModels.Base;

namespace SmithereenUWP.ViewModels
{
    public class ProfileViewModel : ItemsViewModel<WallPost>
    {
        private readonly int _id;

        public ProfileViewModel() : this(0) { }

        public ProfileViewModel(int id)
        {
            if (id <= 0) _id = AppParameters.CurrentUserId;
        }
    }
}
