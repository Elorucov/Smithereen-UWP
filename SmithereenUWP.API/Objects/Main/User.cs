using Newtonsoft.Json;
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

    public sealed class User : IWithAvatar
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("first_name")]
        public string FirstName { get; private set; }

        [JsonProperty("last_name")]
        public string LastName { get; private set; }

        [JsonProperty("deactivated")]
        public UserDeactivationType Deactivated { get; private set; }

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

        [JsonProperty("online")]
        public bool Online { get; private set; }

        [JsonProperty("online_mobile")]
        public bool OnlineMobile { get; private set; }

        [JsonProperty("photo_50")]
        public string Photo50 { get; private set; }

        [JsonProperty("photo_100")]
        public string Photo100 { get; private set; }

        [JsonProperty("photo_200")]
        public string Photo200 { get; private set; }
    }
}
