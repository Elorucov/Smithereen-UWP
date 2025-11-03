using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Objects.Main
{
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
    }
}
