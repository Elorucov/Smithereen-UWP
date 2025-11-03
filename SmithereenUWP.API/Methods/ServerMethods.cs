using SmithereenUWP.API.Objects.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Methods
{
    public sealed class ServerMethods : MethodSectionBase
    {
        internal ServerMethods(SmithereenAPI api) : base("server", api) {}

        public async Task<ServerInfo> GetInfoAsync()
        {
            return await CallMethodAsync<ServerInfo>("getInfo");
        }
    }
}
