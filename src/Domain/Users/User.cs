using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class User : EntityBase
    {
        public const int ACCOUNT_MIN_LENGTH = 2;

        private User(DB.Tables.User userModel)
        {
            if (userModel is null)
                throw new ArgumentNullException("user model not found");
            Id = userModel.Id;
            _name = userModel.Name;

            UserCache.SetUserModel(userModel);
        }

        private readonly string _name = "";

        /// <summary>
        /// user name
        /// </summary>
        public string Name => _name;
        /// <summary>
        /// return user name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
        /// <summary>
        /// user's hash code is Id
        /// </summary>
        public override int GetHashCode() => Id;

        public static User Parse(DB.Tables.User userModel)
        {
            return new User(userModel);
        }

        /// <summary>
        /// set the user email
        /// </summary>
        /// <returns></returns>
        public async Task<(bool, string)> SetEmailAsync(string email)
        {
            DB.Tables.User user = await UserCache.GetUserModelAsync(Id);
            if (user is null)
                return (false, "该用户不存在");
            await using var db = new LOPDbContext();
            if (await db.Users.AnyAsync(user => user.Id != Id && user.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                return (false, "已经被使用的邮箱");

            user.Email = email;
            db.Users.Update(user);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return (true, "");
            return (false, "");
        }
    }
}
