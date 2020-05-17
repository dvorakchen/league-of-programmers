using Common;
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Users
{
    /// <summary>
    /// user cache
    /// </summary>
    internal static class UserCache
    {
        private const string USER_CACHE_KEY = "6930a2a7-7614-4d03-b6d8-f29d1cca28d2";

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
            value = await db.Users.FirstOrDefaultAsync(user => user.Id == id);
            Cache.Set(key, value, TimeSpan.FromSeconds(30));
            return value;
        }
    }
}
