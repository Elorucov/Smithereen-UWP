using SmithereenUWP.API.Objects.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Methods
{
    public sealed class UtilsMethods : MethodSectionBase
    {
        internal UtilsMethods(SmithereenAPI api) : base("utils", api) { }

        public async Task<UtilsLoadRemoteObjectResponse> LoadRemoteObjectAsync(string q)
        {
            Dictionary<string, string> p = new Dictionary<string, string>()
            {
                {   "q", q  }
            };
            return await CallMethodAsync<UtilsLoadRemoteObjectResponse>("loadRemoteObject", p);
        }
    }
}
