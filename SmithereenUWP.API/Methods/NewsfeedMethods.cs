using SmithereenUWP.API.Objects.Main;
using SmithereenUWP.API.Objects.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Methods
{
    public sealed class NewsfeedMethods : MethodSectionBase
    {
        internal NewsfeedMethods(SmithereenAPI api) : base("newsfeed", api) { }

        public async Task<NewsfeedGetResponse> GetAsync(List<string> filters, int count, string startFrom, bool returnBanned = false)
        {
            Dictionary<string, string> p = new Dictionary<string, string>()
            {
                {   "filters", string.Join(",", filters)  },
                {   "count", count.ToString()  }
            };
            if (!string.IsNullOrEmpty(startFrom)) p.Add("start_from", startFrom);
            if (returnBanned) p.Add("return_banned", "true");

            return await CallMethodAsync<NewsfeedGetResponse>("get", p);
        }
    }
}
