using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Objects.Main
{
    public enum GroupAccessType
    {
        [EnumMember(Value = "closed")]
        Closed,

        [EnumMember(Value = "open")]
        Open,

        [EnumMember(Value = "private")]
        Private
    }

    public enum GroupType
    {
        [EnumMember(Value = "group")]
        Group,

        [EnumMember(Value = "event")]
        Event
    }

    public enum GroupAdminLevel
    {
        [EnumMember(Value = "moderator")]
        Moderator,

        [EnumMember(Value = "admin")]
        Admin,

        [EnumMember(Value = "owner")]
        Owner
    }

    public sealed class Group
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("deactivated")]
        public UserDeactivationType Deactivated { get; private set; }

        [JsonProperty("ap_id")]
        public string ActivityPubId { get; private set; }

        [JsonProperty("access_type")]
        public GroupAccessType AccessType { get; private set; }

        [JsonProperty("type")]
        public GroupType Type { get; private set; }

        [JsonProperty("domain")]
        public string Domain { get; private set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; private set; }

        [JsonProperty("status")]
        public string Status { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("is_admin")]
        public bool IsAdmin { get; private set; }

        [JsonProperty("admin_level")]
        public GroupAdminLevel AdminLevel { get; private set; }

        [JsonProperty("is_member")]
        public bool IsMember { get; private set; }
    }
}
