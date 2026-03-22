using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace SmithereenUWP.API.Objects.Response
{
    public enum FediverseObjectType
    {
        [EnumMember(Value = "user")]
        User,

        [EnumMember(Value = "group")]
        Group,

        [EnumMember(Value = "wall_post")]
        WallPost,

        [EnumMember(Value = "wall_comment")]
        WallComment,

        [EnumMember(Value = "photo_album")]
        PhotoAlbum,

        [EnumMember(Value = "photo")]
        Photo,

        [EnumMember(Value = "comment")]
        Comment,

        [EnumMember(Value = "topic")]
        Topic
    }

    public enum ParentFediverseObjectType
    {
        [EnumMember(Value = "wall_post")]
        WallPost,

        [EnumMember(Value = "photo")]
        Photo,

        [EnumMember(Value = "topic")]
        Topic
    }

    public sealed class UtilsLoadRemoteObjectResponse
    {
        [JsonProperty("type")]
        public FediverseObjectType Type { get; private set; }

        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("parent_type")]
        public ParentFediverseObjectType? ParentType { get; private set; }

        [JsonProperty("parent_id")]
        public string ParentId { get; private set; }
    }
}
