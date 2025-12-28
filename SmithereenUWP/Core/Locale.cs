using System;
using Windows.ApplicationModel.Resources;

namespace SmithereenUWP.Core
{
    internal class Locale
    {
        static ResourceLoader loader = new ResourceLoader();

        public static string Get(string key)
        {
            string l = loader.GetString(key);
            return String.IsNullOrEmpty(l) ? $"%{key}%" : l;
        }

        public static string GetOrEmpty(string key)
        {
            return loader.GetString(key);
        }
    }
}
