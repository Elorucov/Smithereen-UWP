using System.Collections.Generic;

namespace SmithereenUWP.Core
{
    public class Constants
    {
        public const string OAUTH_CLIENT_ID = "https://elor.top/smithewin/config.json";
        public const string OAUTH_REDIRECT_URI = "smithewin://oauth-callback";

        public static readonly IReadOnlyList<string> OAuthScopes = new List<string>
        {
            "friends", "photos", "account", "wall", "groups", "messages", "likes", "newsfeed", "notifications", "offline"
        }.AsReadOnly();

        public static readonly IReadOnlyList<string> RequiredUserAndGroupFields = new List<string>
        {
            "photo_100"
        }.AsReadOnly();
    }
}
