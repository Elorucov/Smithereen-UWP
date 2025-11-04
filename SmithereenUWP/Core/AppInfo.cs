using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;

namespace SmithereenUWP.Core
{
    public class AppInfo
    {
        public static string VersionString => GetVersion();
        public static string UserAgent => GetUserAgent();

        private static string GetVersion()
        {
            var major = Package.Current.Id.Version.Major;
            var minor = Package.Current.Id.Version.Minor;
            var build = Package.Current.Id.Version.Build;
            return $"{major}.{minor}.{build}";
        }

        private static string GetUserAgent()
        {
            return $"SmithereenUWP {GetVersion()}";
        }
    }
}
