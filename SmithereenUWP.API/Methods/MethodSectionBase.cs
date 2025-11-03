using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmithereenUWP.API.Methods
{
    public class MethodSectionBase
    {
        private readonly string _name;
        private readonly SmithereenAPI _api;

        internal MethodSectionBase(string name, SmithereenAPI api)
        {
            _name = name;
            _api = api;
        }

        internal async Task<T> CallMethodAsync<T>(string method, Dictionary<string, string> parameters = null)
        {
            return await _api.CallMethodAsync<T>($"{_name}.{method}", parameters);
        }
    }
}
