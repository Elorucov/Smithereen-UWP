using Jeffijoe.MessageFormat;
using SmithereenUWP.API.Objects.Main;
using SmithereenUWP.Core;
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

        public static string GetOnlineStatus(this User user, bool toLower = false)
        {
            string status = string.Empty;
            if (user.Online)
            {
                status = Locale.Get("online");
            }
            else if (user.LastSeen != null)
            {
                status = MessageFormatter.Format(Locale.Get("last_seen"), new
                {
                    time = DateTimeOffset.FromUnixTimeSeconds(user.LastSeen.Time).DateTime.ToRelativeOrAbsoluteTime(),
                    gender = user.Sex == UserSex.Female ? "female" : "male"
                });
            }
            // Foreign users doesn't support online info,
            // so both fields are not returned from API.
            return status;
        }
    }
}
