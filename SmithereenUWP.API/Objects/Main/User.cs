using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SmithereenUWP.API.Objects.Main
{
    public enum UserDeactivationType
    {
        [EnumMember(Value = "banned")]
        Banned,

        [EnumMember(Value = "deleted")]
        Deleted,

        [EnumMember(Value = "hidden")]
        Hidden
    }

    public enum UserSex
    {
        [EnumMember(Value = "female")]
        Female,

        [EnumMember(Value = "male")]
        Male,

        [EnumMember(Value = "other")]
        Other
    }

    public enum UserRelation
    {
        [EnumMember(Value = "actively_searching")]
        ActivelySearching,

        [EnumMember(Value = "complicated")]
        Complicated,

        [EnumMember(Value = "engaged")]
        Engaged,

        [EnumMember(Value = "in_love")]
        InLove,

        [EnumMember(Value = "in_relationship")]
        InRelationship,

        [EnumMember(Value = "married")]
        Married,

        [EnumMember(Value = "single")]
        Single
    }

    public enum UserPoliticalView
    {
        [EnumMember(Value = "apathetic")]
        Apathetic,

        [EnumMember(Value = "communist")]
        Communist,

        [EnumMember(Value = "socialist")]
        Socialist,

        [EnumMember(Value = "moderate")]
        Moderate,

        [EnumMember(Value = "liberal")]
        Liberal,

        [EnumMember(Value = "conservative")]
        Conservative,

        [EnumMember(Value = "monarchist")]
        Monarchist,

        [EnumMember(Value = "ultraconservative")]
        Ultraconservative,

        [EnumMember(Value = "libertarian")]
        Libertarian
    }

    public enum UserPeopleMain
    {
        [EnumMember(Value = "intellect_creativity")]
        IntellectCreativity,

        [EnumMember(Value = "kindness_honesty")]
        KindnessHonesty,

        [EnumMember(Value = "health_beauty")]
        HealthBeauty,

        [EnumMember(Value = "wealth_power")]
        WealthPower,

        [EnumMember(Value = "courage_persistence")]
        CouragePersistence,

        [EnumMember(Value = "humor_life_love")]
        HumorLifeLove
    }

    public enum UserLifeMain
    {
        [EnumMember(Value = "family_children")]
        FamilyChildren,

        [EnumMember(Value = "career_money")]
        CareerMoney,

        [EnumMember(Value = "entertainment_leisure")]
        EntertainmentLeisure,

        [EnumMember(Value = "science_research")]
        ScienceResearch,

        [EnumMember(Value = "improving_world")]
        ImprovingWorld,

        [EnumMember(Value = "personal_development")]
        PersonalDevelopment,

        [EnumMember(Value = "beauty_art")]
        BeautyArt,

        [EnumMember(Value = "fame_influence")]
        FameInfluence
    }

    public enum UserView
    {
        [EnumMember(Value = "very_negative")]
        VeryNegative,

        [EnumMember(Value = "negative")]
        Negative,

        [EnumMember(Value = "tolerant")]
        Tolerant,

        [EnumMember(Value = "neutral")]
        Neutral,

        [EnumMember(Value = "positive")]
        Positive
    }

    public enum OnlinePlatform
    {
        [EnumMember(Value = "desktop")]
        Desktop,

        [EnumMember(Value = "mobile")]
        Mobile
    }

    public enum UserFriendStatus
    {
        [EnumMember(Value = "none")]
        None,

        [EnumMember(Value = "friends")]
        Friends,

        [EnumMember(Value = "following")]
        Following,

        [EnumMember(Value = "followed_by")]
        FollowedBy,

        [EnumMember(Value = "follow_requested")]
        FollowRequested,
    }

    public enum UserWallDefault
    {
        [EnumMember(Value = "owner")]
        Owner,

        [EnumMember(Value = "all")]
        All
    }

    public sealed class ProfileField
    {
        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("value")]
        public string Value { get; private set; }
    }

    //public sealed class UserConnections
    //{
    //    [JsonProperty("matrix")]
    //    public string Matrix { get; private set; }

    //    [JsonProperty("xmpp")]
    //    public string XMPP { get; private set; }

    //    [JsonProperty("telegram")]
    //    public string Telegram { get; private set; }

    //    [JsonProperty("signal")]
    //    public string Signal { get; private set; }

    //    [JsonProperty("twitter")]
    //    public string Twitter { get; private set; }

    //    [JsonProperty("instagram")]
    //    public string Instagram { get; private set; }

    //    [JsonProperty("facebook")]
    //    public string Facebook { get; private set; }

    //    [JsonProperty("vkontakte")]
    //    public string VKontakte { get; private set; }
    //}

    public sealed class UserPersonal
    {
        [JsonProperty("political")]
        public UserPoliticalView? Political { get; private set; }

        [JsonProperty("religion")]
        public string Religion { get; private set; }

        [JsonProperty("inspired_by")]
        public string InspiredBy { get; private set; }

        [JsonProperty("people_main")]
        public UserPeopleMain? PeopleMain { get; private set; }

        [JsonProperty("life_main")]
        public UserLifeMain? LifeMain { get; private set; }

        [JsonProperty("smoking")]
        public UserView? Smoking { get; private set; }

        [JsonProperty("alcohol")]
        public UserView? Alcohol { get; private set; }
    }

    public sealed class UserLastSeen
    {
        [JsonProperty("time")]
        public long Time { get; private set; }

        [JsonProperty("platform")]
        public OnlinePlatform Platform { get; private set; }

        [JsonProperty("app_id")]
        public int AppId { get; private set; }
    }

    public sealed class UserCounters
    {
        [JsonProperty("albums")]
        public int Albums { get; private set; }

        [JsonProperty("photos")]
        public int Photos { get; private set; }

        [JsonProperty("friends")]
        public int Friends { get; private set; }

        [JsonProperty("groups")]
        public int Groups { get; private set; }

        [JsonProperty("online_friends")]
        public int OnlineFriends { get; private set; }

        [JsonProperty("mutual_friends")]
        public int MutualFriends { get; private set; }

        [JsonProperty("user_photos")]
        public int UserPhotos { get; private set; }

        [JsonProperty("followers")]
        public int Followers { get; private set; }

        [JsonProperty("subscriptions")]
        public int Subscriptions { get; private set; }
    }

    public sealed class User : IWithAvatar
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("first_name")]
        public string FirstName { get; private set; }

        [JsonProperty("last_name")]
        public string LastName { get; private set; }

        [JsonProperty("deactivated")]
        public UserDeactivationType? Deactivated { get; private set; }

        [JsonProperty("ap_id")]
        public string ActivityPubId { get; private set; }

        [JsonProperty("domain")]
        public string Domain { get; private set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("nickname")]
        public string Nickname { get; private set; }

        [JsonProperty("maiden_name")]
        public string MaidenName { get; private set; }

        [JsonProperty("sex")]
        public UserSex Sex { get; private set; }

        [JsonProperty("bdate")]
        public string Birthdate { get; private set; }

        [JsonProperty("home_town")]
        public string HomeTown { get; private set; }

        [JsonProperty("relation")]
        public UserRelation? Relation { get; private set; }

        [JsonProperty("relation_partner")]
        public User RelationPartner { get; private set; }   // Returns only id, first_name and last_name!

        [JsonProperty("custom")]
        public List<ProfileField> Custom { get; private set; }

        [JsonProperty("city")]
        public string City { get; private set; }

        [JsonProperty("connections")]
        public Dictionary<string, string> Connections { get; private set; }

        [JsonProperty("site")]
        public string Site { get; private set; }

        [JsonProperty("activities")]
        public string Activities { get; private set; }

        [JsonProperty("interests")]
        public string Interests { get; private set; }

        [JsonProperty("music")]
        public string Music { get; private set; }

        [JsonProperty("movies")]
        public string Movies { get; private set; }

        [JsonProperty("tv")]
        public string TV { get; private set; }

        [JsonProperty("books")]
        public string Books { get; private set; }

        [JsonProperty("games")]
        public string Games { get; private set; }

        [JsonProperty("quotes")]
        public string Quotes { get; private set; }

        [JsonProperty("about")]
        public string About { get; private set; }

        [JsonProperty("personal")]
        public UserPersonal Personal { get; private set; }

        [JsonProperty("online")]
        public bool Online { get; private set; }

        [JsonProperty("online_mobile")]
        public bool OnlineMobile { get; private set; }

        [JsonProperty("online_app_id")]
        public int OnlineAppId { get; private set; }

        [JsonProperty("last_seen")]
        public UserLastSeen LastSeen { get; private set; }

        [JsonProperty("blocked")]
        public bool Blocked { get; private set; }

        [JsonProperty("blocked_by_me")]
        public bool BlockedByMe { get; private set; }

        [JsonProperty("can_post")]
        public bool CanPost { get; private set; }

        [JsonProperty("can_see_all_posts")]
        public bool CanSeeAllPosts { get; private set; }

        [JsonProperty("can_send_friend_request")]
        public bool CanSendFriendRequest { get; private set; }

        [JsonProperty("can_write_private_message")]
        public bool CanWritePrivateMessage { get; private set; }

        [JsonProperty("mutual_count")]
        public int MutualCount { get; private set; }

        [JsonProperty("is_friend")]
        public bool IsFriend { get; private set; }

        [JsonProperty("friend_status")]
        public UserFriendStatus? FriendStatus { get; private set; }

        [JsonProperty("is_favorite")]
        public bool IsFavorite { get; private set; }

        [JsonProperty("lists")]
        public List<int> Lists { get; private set; }

        [JsonProperty("is_hidden_from_feed")]
        public bool IsHiddenFromFeed { get; private set; }

        [JsonProperty("wall_default")]
        public UserWallDefault WallDefault { get; private set; }

        [JsonProperty("photo_50")]
        public string Photo50 { get; private set; }

        [JsonProperty("photo_100")]
        public string Photo100 { get; private set; }

        [JsonProperty("photo_200")]
        public string Photo200 { get; private set; }

        [JsonProperty("photo_400")]
        public string Photo400 { get; private set; }

        [JsonProperty("photo_400_orig")]
        public string Photo400Orig { get; private set; }

        [JsonProperty("photo_id")]
        public string PhotoId { get; private set; }

        [JsonProperty("has_photo")]
        public bool HasPhoto { get; private set; }

        [JsonProperty("counters")]
        public UserCounters Counters { get; private set; }
    }
}
