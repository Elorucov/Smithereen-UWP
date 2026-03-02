using Newtonsoft.Json;
using SmithereenUWP.API.Objects.Main;
using System.Collections.Generic;

namespace SmithereenUWP.DataModels.ExecuteAPI
{
    public sealed class UserProfileResponse
    {
        [JsonProperty("user")]
        public User User { get; private set; }

        [JsonProperty("mutual_friends_ids")]
        public List<int> MutualFriendsIDs { get; private set; }

        [JsonProperty("friends_ids")]
        public List<int> FriendsIDs { get; private set; }

        [JsonProperty("online_friends_ids")]
        public List<int> OnlineFriendsIDs { get; private set; }

        [JsonProperty("photo_albums")]
        public List<PhotoAlbum> PhotoAlbums { get; private set; }

        [JsonProperty("subscriptions_ids")]
        public List<int> SubscriptionsIDs { get; private set; }

        [JsonProperty("groups")]
        public List<Group> Groups { get; private set; }

        [JsonProperty("mentioned_users")]
        public List<User> MentionedUsers { get; private set; }
    }
}
