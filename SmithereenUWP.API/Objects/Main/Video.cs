using Newtonsoft.Json;

namespace SmithereenUWP.API.Objects.Main
{
    public sealed class Video : ISizedAttachment
    {
        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("width")]
        public int Width { get; private set; }

        [JsonProperty("height")]
        public int Height { get; private set; }

        [JsonProperty("blurhash")]
        public string BlurHash { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }
    }
}
