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
        static Dictionary<int, Group> _groups = new Dictionary<int, Group>();

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

        public static void CacheGroup(Group group)
        {
            if (_groups.ContainsKey(group.Id))
            {
                _groups[group.Id] = group;
            }
            else
            {
                _groups.Add(group.Id, group);
            }
        }

        public static void CacheGroups(IEnumerable<Group> groups)
        {
            for (int i = 0; i < groups.Count(); i++)
            {
                CacheGroup(groups.ElementAt(i));
            }
        }

        public static Group GetCachedGroup(int id)
        {
            if (_groups.ContainsKey(id))
            {
                return _groups[id];
            }
            return null;
        }
    }
}
