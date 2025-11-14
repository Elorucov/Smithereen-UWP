using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
