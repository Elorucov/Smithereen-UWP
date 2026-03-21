using SmithereenUWP.API.Objects.Main;
using SmithereenUWP.Core;
using SmithereenUWP.DataModels;
using SmithereenUWP.Extensions;
using SmithereenUWP.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public bool ItsMe => AppParameters.CurrentUserId == User.Id;

        // Info entities
        public List<EntityWithIcon> InfosAlwaysShownInNarrowMode { get; } = new List<EntityWithIcon>(); // status, friends/followers counters on narrow mode
        public List<Entity> ImportantInfos { get; } = new List<Entity>(); // birthday and relation
        public List<Entity> ContactInfos { get; } = new List<Entity>();
        public List<Entity> UserViews { get; } = new List<Entity>();
        public List<Entity> PersonalInfo { get; } = new List<Entity>();


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

                User = null;

                var response = await SessionViewModel.Current.API.GetUserProfileAsync(_id);
                MutualFriends = response.MentionedUsers.Where(u => new HashSet<int>(response.MutualFriendsIDs).Contains(u.Id)).ToList();
                Friends = response.MentionedUsers.Where(u => new HashSet<int>(response.FriendsIDs).Contains(u.Id)).ToList();
                OnlineFriends = response.MentionedUsers.Where(u => new HashSet<int>(response.OnlineFriendsIDs).Contains(u.Id)).ToList();
                PhotoAlbums = response.PhotoAlbums;
                Subscriptions = response.MentionedUsers.Where(u => new HashSet<int>(response.SubscriptionsIDs).Contains(u.Id)).ToList();
                Groups = response.Groups;

                User = response.User;
                OnlineInfo = User.GetOnlineStatus().ToLower();

                UpdateInfos();
                OnPropertyChanged(nameof(ItsMe));
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

        private void UpdateInfos()
        {
            InfosAlwaysShownInNarrowMode.Clear();
            ImportantInfos.Clear();
            ContactInfos.Clear();
            UserViews.Clear();
            PersonalInfo.Clear();

            if (!string.IsNullOrEmpty(User.Status))
                InfosAlwaysShownInNarrowMode.Add(new EntityWithIcon(0, '', "Status", User.Status));

            if (User.Counters != null && User.Counters.Friends > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<p>");
                sb.Append($"<b>{User.Counters.Friends}</b> friends");

                if (User.Counters.MutualFriends > 0)
                {
                    sb.Append(" · ");
                    sb.Append($"<b>{User.Counters.MutualFriends}</b> mutual");
                }

                sb.Append("</p>");

                InfosAlwaysShownInNarrowMode.Add(new EntityWithIcon(0, '', "Friends", sb.ToString()));

                if (User.Counters.Followers > 0)
                    InfosAlwaysShownInNarrowMode.Add(new EntityWithIcon(0, '', "Followers", $"<p><b>{User.Counters.Followers}</b> followers</p>"));
            }

            if (!string.IsNullOrEmpty(User.Birthdate))
                ImportantInfos.Add(new Entity(0, "Birthdate", User.Birthdate));

            if (!string.IsNullOrEmpty(User.HomeTown))
                ImportantInfos.Add(new Entity(0, "Home town", User.HomeTown));

            if (User.Relation.HasValue)
                ImportantInfos.Add(new Entity(0, "Relation", User.Relation.ToString()));

            // Contacts

            if (!string.IsNullOrEmpty(User.City))
                ContactInfos.Add(new Entity(0, "City", User.City));

            if (!string.IsNullOrEmpty(User.Site))
                ContactInfos.Add(new Entity(0, "Site", User.Site));

            foreach (var conn in User.Connections)
            {
                ContactInfos.Add(new Entity(0, conn.Key, conn.Value));
            }

            // Views

            if (!string.IsNullOrEmpty(User.Personal?.Religion))
                UserViews.Add(new Entity(0, "Religion", User.Personal.Religion));

            if (User.Personal?.LifeMain.HasValue == true)
                UserViews.Add(new Entity(0, "Personal priority", User.Personal.LifeMain.ToString()));

            if (User.Personal?.PeopleMain.HasValue == true)
                UserViews.Add(new Entity(0, "Important in others", User.Personal.PeopleMain.ToString()));

            if (User.Personal?.Smoking.HasValue == true)
                UserViews.Add(new Entity(0, "Views on smoking", User.Personal.Smoking.ToString()));

            if (User.Personal?.Alcohol.HasValue == true)
                UserViews.Add(new Entity(0, "Views on alcohol", User.Personal.Alcohol.ToString()));

            if (!string.IsNullOrEmpty(User.Personal?.InspiredBy))
                UserViews.Add(new Entity(0, "Inspired by", User.Personal.InspiredBy));

            // Personal info

            if (!string.IsNullOrEmpty(User.Activities))
                PersonalInfo.Add(new Entity(0, "Activities", User.Activities));

            if (!string.IsNullOrEmpty(User.Interests))
                PersonalInfo.Add(new Entity(0, "Interests", User.Interests));

            if (!string.IsNullOrEmpty(User.Books))
                PersonalInfo.Add(new Entity(0, "Favorite books", User.Books));

            if (!string.IsNullOrEmpty(User.TV))
                PersonalInfo.Add(new Entity(0, "Favorite TV shows", User.TV));

            if (!string.IsNullOrEmpty(User.Games))
                PersonalInfo.Add(new Entity(0, "Favorite games", User.Games));

            if (!string.IsNullOrEmpty(User.Quotes))
                PersonalInfo.Add(new Entity(0, "Favorite quotes", User.Quotes));

            if (!string.IsNullOrEmpty(User.About))
                PersonalInfo.Add(new Entity(0, "About", User.About));

            OnPropertyChanged(nameof(ImportantInfos));
            OnPropertyChanged(nameof(ContactInfos));
            OnPropertyChanged(nameof(UserViews));
            OnPropertyChanged(nameof(PersonalInfo));
        }
    }
}
