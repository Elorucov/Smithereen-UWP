using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Objects.Main
{
    public enum AttachmentType
    {
        [EnumMember(Value = "photo")]
        Photo,

        [EnumMember(Value = "graffiti")]
        Graffiti,

        [EnumMember(Value = "video")]
        Video,

        [EnumMember(Value = "audio")]
        Audio,

        [EnumMember(Value = "poll")]
        Poll
    }

    public sealed class Attachment
    {
        [JsonProperty("type")]
        public AttachmentType Type { get; private set; }

        [JsonProperty("photo")]
        public Photo Photo { get; private set; }

        [JsonProperty("graffiti")]
        public Graffiti Graffiti { get; private set; }

        [JsonProperty("video")]
        public Video Video { get; private set; }

        [JsonProperty("audio")]
        public Audio Audio { get; private set; }

        [JsonProperty("poll")]
        public Poll Poll { get; private set; }
    }
}
