using Common;
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Domain.Users
{
    /// <summary>
    /// user cache
    /// </summary>
    internal static class UserCache
    {
        private const string USER_CACHE_KEY = "cachekey690a2a7";

        /// <summary>
        /// the default cache time span
        /// </summary>
        private static readonly TimeSpan Default_Cache_Time = TimeSpan.FromSeconds(10);

        /// <summary>
        /// get user DB model by user account, 
        /// if not in cache, then will get the new model,
        /// if null, then cache too
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeAvatar">is need include avatar</param>
        /// <returns></returns>
        internal static async Task<DB.Tables.User> GetUserModelAsync(string account, bool includeAvatar = false)
        {
            string key = USER_CACHE_KEY + account;
            (bool hasValue, DB.Tables.User value) = Cache.TryGet<DB.Tables.User>(key);
            using var db = new LOPDbContext();

            if (hasValue)
            {
                if (includeAvatar)
                {
                    if (value.Avatar != null)
                        return value;
                    value = await db.Users.AsNoTracking().Include(user => user.Avatar).FirstOrDefaultAsync(user => user.Account == account);
                    Cache.Set(key, value, Default_Cache_Time);
                }
                return value;
            }
            else
            {
                if (includeAvatar)
                    value = await db.Users.AsNoTracking().Include(user => user.Avatar).FirstOrDefaultAsync(user => user.Account == account);
                else
                    value = await db.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Account == account);
                Cache.Set(key, value, Default_Cache_Time);
            }
            return value;
        }

        /// <summary>
        /// cache user model with default cache time span
        /// </summary>
        /// <param name="userModel"></param>
        internal static void SetUserModel(DB.Tables.User userModel)
        {
            SetUserModel(userModel, Default_Cache_Time);
        }

        /// <summary>
        /// cache user model with cache time span
        /// </summary>
        /// <param name="userModel"></param>
        internal static void SetUserModel(DB.Tables.User userModel, TimeSpan cacheTime)
        {
            if (userModel is null)
                throw new ArgumentNullException("cache user model should not null");

            Cache.Set(USER_CACHE_KEY + userModel.Account, userModel, cacheTime);
        }

        /// <summary>
        /// remove the user model by id
        /// </summary>
        /// <param name="id"></param>
        internal static void Remove(string account)
        {
            Cache.Remove(USER_CACHE_KEY + account);
        }
    }
}
