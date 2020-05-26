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
        private const string USER_CACHE_KEY = "6930a2a7";

        /// <summary>
        /// the default cache time span
        /// </summary>
        private static readonly TimeSpan Default_Cache_Time = TimeSpan.FromSeconds(30);

        /// <summary>
        /// get user DB model by user id, 
        /// if not in cache, then will get the new model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal static async Task<DB.Tables.User> GetUserModelAsync(int id)
        {
            string key = USER_CACHE_KEY + id;
            (bool hasValue, DB.Tables.User value) = Cache.TryGet<DB.Tables.User>(key);
            if (hasValue)
                return value;
            using var db = new LOPDbContext();
            value = await db.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
            Cache.Set(key, value, Default_Cache_Time);
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

            Cache.Set(USER_CACHE_KEY + userModel.Id, userModel, cacheTime);
        }

        /// <summary>
        /// remove the user model by id
        /// </summary>
        /// <param name="id"></param>
        internal static void Remove(int id)
        {
            Cache.Remove(USER_CACHE_KEY + id);
        }
    }
}
