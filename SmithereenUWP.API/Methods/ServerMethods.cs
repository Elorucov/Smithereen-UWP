using SmithereenUWP.API.Objects.Main;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Methods
{
    public sealed class ServerMethods : MethodSectionBase
    {
        internal ServerMethods(SmithereenAPI api) : base("server", api) { }

        public async Task<ServerInfo> GetInfoAsync()
        {
            return await CallMethodAsync<ServerInfo>("getInfo");
        }
    }
}
