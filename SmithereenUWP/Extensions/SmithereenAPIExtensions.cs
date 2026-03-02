using SmithereenUWP.API;
using SmithereenUWP.Core;
using SmithereenUWP.DataModels.ExecuteAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace SmithereenUWP.Extensions
{
    public static class SmithereenAPIExtensions
    {
        public static async Task<T> ExecuteAsync<T>(this SmithereenAPI api, string name, Dictionary<string, string> parameters = null)
        {
            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/ExecuteScripts/{name}.txt"));
            string code = await FileIO.ReadTextAsync(file);

            if (parameters == null) parameters = new Dictionary<string, string>();
            parameters.Add("code", code);
            return await api.CallMethodAsync<T>("execute", parameters);
        }

        public static async Task<UserProfileResponse> GetUserProfileAsync(this SmithereenAPI api, int id)
        {
            return await api.ExecuteAsync<UserProfileResponse>("userProfile", new Dictionary<string, string>
            {
                { "id", id.ToString() },
                { "need_mutual_friends", AppParameters.CurrentUserId != id ? "1" : "0" }
            });
        }
    }
}
