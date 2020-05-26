using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class User : EntityBase
    {
        public const int ACCOUNT_MIN_LENGTH = 2;

        public const int AVATAR_MAX_BYTES_COUNT = 64;

        private User(DB.Tables.User userModel)
        {
            if (userModel is null)
                throw new ArgumentNullException("user model not found");
            Id = userModel.Id;
            Name = userModel.Name;

            UserCache.SetUserModel(userModel);
        }

        /// <summary>
        /// user name
        /// </summary>
        public string Name { get; private set; }
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
        /// modify the user nama
        /// </summary>
        /// <returns></returns>
        public async Task<(bool, string)> ModifyNameAsync(string name)
        {
            DB.Tables.User user = await UserCache.GetUserModelAsync(Id);
            if (user is null)
                return (false, "该用户不存在");
            if (user.Name == name)
                return (true, "");
            await using var db = new LOPDbContext();
            user.Name = name;
            db.Users.Update(user);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
            {
                Name = name;
                return (true, "");
            }
            throw new Exception("修改邮箱失败");
        }

        /// <summary>
        /// modify the user email
        /// </summary>
        /// <returns></returns>
        public async Task<(bool, string)> ModifyEmailAsync(string email)
        {
            DB.Tables.User user = await UserCache.GetUserModelAsync(Id);
            if (user is null)
                return (false, "该用户不存在");
            if (user.Email == email)
                return (true, "");
            await using var db = new LOPDbContext();
            if (await db.Users.AnyAsync(user => user.Id != Id && user.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                return (false, "已经被使用的邮箱");

            user.Email = email;
            db.Users.Update(user);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return (true, "");
            throw new Exception("修改邮箱失败");
        }

        /// <summary>
        /// modify the user email
        /// </summary>
        /// <returns></returns>
        public async Task<(bool, string)> ModifyPasswordAsync(string password)
        {
            DB.Tables.User user = await UserCache.GetUserModelAsync(Id);
            if (user is null)
                return (false, "该用户不存在");
            if (user.Password == password)
                return (true, "");
            await using var db = new LOPDbContext();
            user.Password = password;
            db.Users.Update(user);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return (true, "");
            throw new Exception("修改密码失败");
        }

        /// <summary>
        /// modify the user avatar
        /// </summary>
        /// <returns></returns>
        public async Task<(bool, string)> ModifyAvatarAsync(byte[] avatar)
        {
            if (avatar.Length > AVATAR_MAX_BYTES_COUNT)
                return (false, $"头像不能大于{AVATAR_MAX_BYTES_COUNT}字节");
            
            DB.Tables.User user = await UserCache.GetUserModelAsync(Id);
            if (user is null)
                return (false, "该用户不存在");
            if (Enumerable.SequenceEqual(user.Avatar, avatar))
                return (true, "");

            await using var db = new LOPDbContext();
            user.Avatar = avatar;
            db.Users.Update(user);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return (true, "");
            throw new Exception("修改头像失败");
        }
    }
}
