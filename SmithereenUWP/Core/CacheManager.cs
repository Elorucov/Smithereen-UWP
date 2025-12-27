using SmithereenUWP.API.Objects.Main;
using SmithereenUWP.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmithereenUWP.Core
{
    internal static class CacheManager
    {
        static Dictionary<int, User> _users = new Dictionary<int, User>();

        public static void CacheUser(User user)
        {
            if (_users.ContainsKey(user.Id))
            {
                _users[user.Id] = user;
            }
            else
            {
                _users.Add(user.Id, user);
            }
        }

        public static void CacheUsers(IEnumerable<User> users)
        {
            for (int i = 0; i < users.Count(); i++)
            {
                CacheUser(users.ElementAt(i));
            }
        }

        public static User GetCachedUser(int id)
        {
            if (_users.ContainsKey(id))
            {
                return _users[id];
            }
            return null;
        }

        public static Tuple<string, string, Uri> GetUserNameAndAvatar(int id)
        {
            var user = GetCachedUser(id);
            if (user != null)
            {
                return new Tuple<string, string, Uri>(user.FirstName, user.LastName, user.GetAvatar());
            }
            return new Tuple<string, string, Uri>("User", id.ToString(), null);
        }
    }
}
