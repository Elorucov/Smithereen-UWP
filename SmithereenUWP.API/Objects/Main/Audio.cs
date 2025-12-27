using Newtonsoft.Json;

namespace SmithereenUWP.API.Objects.Main
{
    public sealed class Audio
    {
        [JsonProperty("url")]
        public string Url { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }
    }
}
