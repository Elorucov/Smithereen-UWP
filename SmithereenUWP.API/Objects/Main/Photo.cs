using Newtonsoft.Json;
using System.Collections.Generic;

namespace SmithereenUWP.API.Objects.Main
{
    public sealed class PhotoSize
    {
        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("width")]
        public int Width { get; private set; }

        [JsonProperty("height")]
        public int Height { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }
    }

    public sealed class Photo : ISizedAttachment
    {
        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("ap_id")]
        public string ActivityPubId { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("album_id")]
        public string AlbumId { get; private set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }

        [JsonProperty("user_id")]
        public int UserId { get; private set; }

        [JsonProperty("text")]
        public string Text { get; private set; }

        [JsonProperty("date")]
        public long Date { get; private set; }

        [JsonProperty("blurhash")]
        public string BlurHash { get; private set; }

        [JsonProperty("has_tags")]
        public bool HasTags { get; private set; }

        [JsonProperty("sizes")]
        public List<PhotoSize> Sizes { get; private set; }

        [JsonProperty("width")]
        public int Width { get; private set; }

        [JsonProperty("height")]
        public int Height { get; private set; }
    }
}
