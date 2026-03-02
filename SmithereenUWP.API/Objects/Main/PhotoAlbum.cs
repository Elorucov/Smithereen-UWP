using Newtonsoft.Json;

namespace SmithereenUWP.API.Objects.Main
{
    public sealed class PhotoAlbum
    {
        [JsonProperty("id")]
        public string Id { get; private set; }

        [JsonProperty("ap_id")]
        public string ActivityPubId { get; private set; }

        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("owner_id")]
        public int OwnerId { get; private set; }

        [JsonProperty("is_system")]
        public bool IsSystem { get; private set; }

        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("cover_id")]
        public string CoverId { get; private set; }

        [JsonProperty("created")]
        public long Created { get; private set; }

        [JsonProperty("updated")]
        public long Updated { get; private set; }

        [JsonProperty("size")]
        public int Size { get; private set; }

        [JsonProperty("cover")]
        public Photo Cover { get; private set; }
    }
}