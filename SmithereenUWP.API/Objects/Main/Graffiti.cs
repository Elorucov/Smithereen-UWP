using Newtonsoft.Json;

namespace SmithereenUWP.API.Objects.Main
{
    public sealed class Graffiti
    {
        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("preview_url")]
        public string PreviewUrl { get; private set; }

        [JsonProperty("width")]
        public int Width { get; private set; }

        [JsonProperty("height")]
        public int Height { get; private set; }
    }
}
