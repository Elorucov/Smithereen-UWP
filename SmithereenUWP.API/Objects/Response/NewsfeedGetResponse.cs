using Newtonsoft.Json;
using SmithereenUWP.API.Objects.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Objects.Response
{
    public enum NewsfeedItemType
    {
        [EnumMember(Value = "board")]
        Board,

        [EnumMember(Value = "event_create")]
        EventCreate,

        [EnumMember(Value = "event_join")]
        EventJoin,

        [EnumMember(Value = "friend")]
        Friend,

        [EnumMember(Value = "group_create")]
        GroupCreate,

        [EnumMember(Value = "group_join")]
        GroupJoin,

        [EnumMember(Value = "photo")]
        Photo,

        [EnumMember(Value = "photo_tag")]
        PhotoTag,

        [EnumMember(Value = "post")]
        Post,

        [EnumMember(Value = "relation")]
        Relation
    }

    public sealed class NewsfeedPhotosInfo
    {
        [JsonProperty("count")]
        public int Count { get; private set; }

        [JsonProperty("items")]
        public List<Photo> Items { get; private set; }

        [JsonProperty("list_id")]
        public string ListId { get; private set; }
    }

    public sealed class NewsfeedRelationInfo
    {
        [JsonProperty("status")]
        public UserRelation Status { get; private set; }

        [JsonProperty("partner")]
        public int Partner { get; private set; }
    }

    public sealed class NewsfeedItem
    {
        [JsonProperty("type")]
        public NewsfeedItemType Type { get; private set; }

        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("user_id")]
        public int UserId { get; private set; }

        [JsonProperty("post")]
        public WallPost Post { get; private set; }

        [JsonProperty("photos")]
        public NewsfeedPhotosInfo Photos { get; private set; }

        [JsonProperty("friend_ids")]
        public List<int> FriendIds { get; private set; }

        [JsonProperty("group_ids")]
        public List<int> GroupIds { get; private set; }

        [JsonProperty("topics")]
        public List<BoardTopic> Topics { get; private set; }

        [JsonProperty("relation")]
        public NewsfeedRelationInfo Relation { get; private set; }
    }

    public sealed class NewsfeedGetResponse
    {
        [JsonProperty("items")]
        public List<NewsfeedItem> Items { get; private set; }

        [JsonProperty("profiles")]
        public List<User> Profiles { get; private set; }

        [JsonProperty("groups")]
        public List<Group> Groups { get; private set; }

        [JsonProperty("next_from")]
        public string NextFrom { get; private set; }
    }
}
