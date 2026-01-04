using Windows.Storage;

namespace SmithereenUWP.Core
{
    public static class AppParameters
    {
        private static readonly ApplicationDataContainer adc = ApplicationData.Current.LocalSettings;

        #region Constants

        const string USER_ID = "uid";
        const string ACCESS_TOKEN = "at";
        const string SERVER = "server";
        const string DEBUG_WALL = "dwall";

        #endregion

        #region Getters

        private static bool GetBoolean(string key)
        {
            return adc.Values[key] != null && adc.Values[key] is bool b ? b : false;
        }

        private static int GetInt32(string key)
        {
            return adc.Values[key] != null && adc.Values[key] is int i ? i : 0;
        }

        private static string GetString(string key)
        {
            return adc.Values[key] != null && adc.Values[key] is string s ? s : null;
        }

        #endregion

        public static int CurrentUserId
        {
            get => GetInt32(USER_ID);
            set => adc.Values[USER_ID] = value;
        }

        public static string CurrentUserAccessToken
        {
            get => GetString(ACCESS_TOKEN);
            set => adc.Values[ACCESS_TOKEN] = value;
        }

        public static string CurrentServer
        {
            get => GetString(SERVER);
            set => adc.Values[SERVER] = value;
        }

        // Debug

        public static bool WallDebug
        {
            get => GetBoolean(DEBUG_WALL);
            set => adc.Values[DEBUG_WALL] = value;
        }
    }
}
