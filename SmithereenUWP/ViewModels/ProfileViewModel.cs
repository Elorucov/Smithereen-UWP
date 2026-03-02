using SmithereenUWP.API.Objects.Main;
using SmithereenUWP.Core;
using SmithereenUWP.Extensions;
using SmithereenUWP.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmithereenUWP.ViewModels
{
    public class ProfileViewModel : CommonViewModel
    {
        private readonly int _id;

        private User _user;
        private List<User> _mutualFriends;
        private List<User> _friends;
        private List<User> _onlineFriends;
        private List<PhotoAlbum> _photoAlbums;
        private List<User> _subscriptions;
        private List<Group> _groups;

        private string _onlineInfo;

        public User User { get { return _user; } private set { _user = value; OnPropertyChanged(); } }
        public List<User> MutualFriends { get { return _mutualFriends; } private set { _mutualFriends = value; OnPropertyChanged(); } }
        public List<User> Friends { get { return _friends; } private set { _friends = value; OnPropertyChanged(); } }
        public List<User> OnlineFriends { get { return _onlineFriends; } private set { _onlineFriends = value; OnPropertyChanged(); } }
        public List<PhotoAlbum> PhotoAlbums { get { return _photoAlbums; } private set { _photoAlbums = value; OnPropertyChanged(); } }
        public List<User> Subscriptions { get { return _subscriptions; } private set { _subscriptions = value; OnPropertyChanged(); } }
        public List<Group> Groups { get { return _groups; } private set { _groups = value; OnPropertyChanged(); } }

        // Microsoft is a company that turns
        // "CEO claims 30% of its new code written by AI" into
        // "Cannot convert 'System.COMObject' into 'System.String'" when binding.
        public string OnlineInfo { get { return _onlineInfo; } private set { _onlineInfo = value; OnPropertyChanged(); } }

        public ProfileViewModel() : this(0) { }

        public ProfileViewModel(int id)
        {
            if (id <= 0) _id = AppParameters.CurrentUserId;
        }

        public async Task RefreshDataAsync()
        {
            try
            {
                IsLoading = true;
                Placeholder = null;

                var response = await SessionViewModel.Current.API.GetUserProfileAsync(_id);
                User = response.User;
                MutualFriends = response.MentionedUsers.Where(u => new HashSet<int>(response.MutualFriendsIDs).Contains(u.Id)).ToList();
                Friends = response.MentionedUsers.Where(u => new HashSet<int>(response.FriendsIDs).Contains(u.Id)).ToList();
                OnlineFriends = response.MentionedUsers.Where(u => new HashSet<int>(response.OnlineFriendsIDs).Contains(u.Id)).ToList();
                PhotoAlbums = response.PhotoAlbums;
                Subscriptions = response.MentionedUsers.Where(u => new HashSet<int>(response.SubscriptionsIDs).Contains(u.Id)).ToList();
                Groups = response.Groups;

                OnlineInfo = User.GetOnlineStatus().ToLower();
            }
            catch (Exception ex)
            {
                Placeholder = PlaceholderViewModel.GetForException(ex, async () => await RefreshDataAsync());
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
