using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SmithereenUWP.Core
{
    public static class AppParameters
    {
        private static readonly ApplicationDataContainer adc = ApplicationData.Current.LocalSettings;

        #region Constants

        const string uid = "uid";
        const string at = "at";
        const string server = "server";

        #endregion

        public static int CurrentUserId
        {
            get => adc.Values[uid] != null && adc.Values[uid] is int i ? i : 0;
            set => adc.Values[uid] = value;
        }

        public static string CurrentUserAccessToken
        {
            get => adc.Values[at] != null && adc.Values[at] is string s ? s : null;
            set => adc.Values[at] = value;
        }

        public static string CurrentServer
        {
            get => adc.Values[server] != null && adc.Values[server] is string s ? s : null;
            set => adc.Values[server] = value;
        }
    }
}
