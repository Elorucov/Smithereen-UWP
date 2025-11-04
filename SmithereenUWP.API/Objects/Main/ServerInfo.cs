using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Objects.Main
{

    public enum ServerSignupMode
    {
        [EnumMember(Value = "closed")]
        Closed,

        [EnumMember(Value = "invite_only")]
        InviteOnly,

        [EnumMember(Value = "manual_approval")]
        ManualApproval,

        [EnumMember(Value = "open")]
        Open
    }

    public sealed class ServerRule
    {
        [JsonProperty("id")]
        public int Id { get; private set; }

        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }
    }

    public sealed class ServerAPIVersion
    {
        [JsonProperty("smithereen")]
        public string Smithereen { get; private set; }
    }

    public sealed class ServerUploads
    {
        [JsonProperty("image_max_size")]
        public int ImageMaxSize { get; private set; }

        [JsonProperty("image_max_dimensions")]
        public int ImageMaxDimensions { get; private set; }

        [JsonProperty("image_types")]
        public List<string> ImageTypes { get; private set; }
    }

    public sealed class ServerStats
    {
        [JsonProperty("users")]
        public int Users { get; private set; }

        [JsonProperty("active_users")]
        public int ActiveUsers { get; private set; }

        [JsonProperty("groups")]
        public int Groups { get; private set; }
    }

    public sealed class ServerInfo
    {
        [JsonProperty("domain")]
        public string Domain { get; private set; }

        [JsonProperty("name")]
        public string Name { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("short_description")]
        public string ShortDescription { get; private set; }

        [JsonProperty("version")]
        public string Version { get; private set; }

        [JsonProperty("admin_email")]
        public string AdminEmail { get; private set; }

        [JsonProperty("rules")]
        public List<ServerRule> Rules { get; private set; }

        [JsonProperty("signup_mode")]
        public ServerSignupMode SignupMode { get; private set; }

        [JsonProperty("api_versions")]
        public ServerAPIVersion APIVersions { get; private set; }

        [JsonProperty("uploads")]
        public ServerUploads Uploads { get; private set; }

        [JsonProperty("stats")]
        public ServerStats Stats { get; private set; }
    }
}
