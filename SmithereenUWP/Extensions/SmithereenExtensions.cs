using SmithereenUWP.API.Objects.Main;
using System;

namespace SmithereenUWP.Extensions
{
    internal static class SmithereenExtensions
    {
        public static Uri GetAvatar(this IWithAvatar obj)
        {
            if (Uri.IsWellFormedUriString(obj.Photo200, UriKind.Absolute)) return new Uri(obj.Photo200);
            if (Uri.IsWellFormedUriString(obj.Photo100, UriKind.Absolute)) return new Uri(obj.Photo100);
            if (Uri.IsWellFormedUriString(obj.Photo50, UriKind.Absolute)) return new Uri(obj.Photo50);
            return null;
        }
    }
}
