using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Objects.Main
{
    public class PaginatedList<T>
    {
        [JsonProperty("items")]
        public List<T> Items { get; private set; }

        [JsonProperty("count")]
        public int Count { get; private set; }

        [JsonProperty("profiles")]
        public List<User> Profiles { get; private set; }

        [JsonProperty("groups")]
        public List<Group> Groups { get; private set; }
    }
}
