/*
 *  用户
 *  
 *  用户既可以是客户，也可以是管理员
 *  所以这个 User 类应该是客户和管理员的抽象类
 *  
 *  Role:   用户的角色, 不可修改
 *  
 */

using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Domain.Users
{
    public abstract class User : EntityBase
    {
        public const int ACCOUNT_MIN_LENGTH = 4;
        public const int ACCOUNT_MAX_LENGTH = 20;

        public const int NAME_MIN_LENGTH = 4;
        public const int NAME_MAX_LENGTH = 20;
        public const string NAME_NOT_ALLOW_CHAR = "\\*/'.,;=?";

        public const int AVATAR_MAX_BYTES_COUNT = 64;

        /// <summary>
        /// 角色分类
        /// </summary>
        [Flags]
        public enum RoleCategories
        {
            Client = 1 << 0,
            Administrator = 2 << 1,
        }

        protected User(DB.Tables.User userModel)
        {
            if (userModel is null)
                throw new ArgumentNullException("user model not found");
            Id = userModel.Id;
            Name = userModel.Name;
            Role = (RoleCategories)userModel.Roles;

            UserCache.SetUserModel(userModel);
        }

        /// <summary>
        /// user name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 角色
        /// </summary>
        public readonly RoleCategories Role;
        /// <summary>
        /// return user name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
        /// <summary>
        /// user's hash code is Id
        /// </summary>
        public override int GetHashCode() => Id;

        /// <summary>
        /// 用户对象只能通过这个方法获取
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        internal static User Parse(DB.Tables.User userModel)
        {
            if (userModel is null)
                throw new NullReferenceException();

            return userModel.Roles switch
            {
                //  如果是管理员
                var r when (r & (int)RoleCategories.Administrator) != 0 => new Administrator(userModel),
                //  如果是客户
                var r when (r & (int)RoleCategories.Client) != 0 => new Client(userModel),
                _ => throw new Exception("未知的角色")
            };
        }

        /// <summary>
        /// modify the user nama
        /// </summary>
        /// <returns></returns>
        public virtual async Task<(bool, string)> ModifyNameAsync(string name)
        {
            name = name.Trim();

            DB.Tables.User user = await UserCache.GetUserModelAsync(Id);
            if (user is null)
                return (false, "该用户不存在");

            if (!new Validation().ValidateUserName(name))
                return (false, $"用户名必须大于{NAME_MIN_LENGTH}位小于{NAME_MAX_LENGTH}位，却不能带有 {NAME_NOT_ALLOW_CHAR}");

            if (user.Name == name)
                return (true, "");
            await using var db = new LOPDbContext();
            user.Name = name;
            db.Users.Update(user);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
            {
                UserCache.SetUserModel(user);
                Name = name;
                return (true, "");
            }
            throw new Exception("修改邮箱失败");
        }

        /// <summary>
        /// modify the user email
        /// </summary>
        /// <returns></returns>
        public virtual async Task<(bool, string)> ModifyEmailAsync(string email)
        {
            email = email.Trim();
            DB.Tables.User user = await UserCache.GetUserModelAsync(Id);
            if (user is null)
                return (false, "该用户不存在");

            if (!new Validation().ValidateEmail(email))
                return (false, "邮箱格式不正确");

            if (user.Email == email)
                return (true, "");
            await using var db = new LOPDbContext();
            if (await db.Users.AnyAsync(user => user.Id != Id && user.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
                return (false, "已经被使用的邮箱");

            user.Email = email;
            db.Users.Update(user);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
            {
                UserCache.SetUserModel(user);
                return (true, "");
            }
            throw new Exception("修改邮箱失败");
        }

        /// <summary>
        /// modify the user email
        /// </summary>
        /// <returns></returns>
        public virtual async Task<(bool, string)> ModifyPasswordAsync(string password)
        {
            DB.Tables.User user = await UserCache.GetUserModelAsync(Id);
            if (user is null)
                return (false, "该用户不存在");

            if (string.IsNullOrWhiteSpace(password))
                return (false, "密码不能为空");

            if (user.Password == password)
                return (true, "");
            await using var db = new LOPDbContext();
            user.Password = password;
            db.Users.Update(user);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
            {
                UserCache.SetUserModel(user);
                return (true, "");
            }
            throw new Exception("修改密码失败");
        }

        /// <summary>
        /// modify the user avatar
        /// </summary>
        /// <returns></returns>
        public virtual async Task<(bool, string)> ModifyAvatarAsync(int avatarId)
        {
            DB.Tables.User user = await UserCache.GetUserModelAsync(Id, true);
            if (user is null)
                return (false, "该用户不存在");
            if (user.AvatarId == avatarId)
                return (true, "");

            await using var db = new LOPDbContext();

            var sourceAvatar = user.Avatar;
            if (sourceAvatar is null)
                sourceAvatar = await db.Files.AsNoTracking().FirstOrDefaultAsync(file => file.Id == user.AvatarId);

            user.AvatarId = avatarId;
            db.Users.Update(user);
            int shouldChangeCount = 1;
            //  删除原头像
            if (sourceAvatar != null)
            {
                db.Files.Remove(sourceAvatar);
                shouldChangeCount++;
            }

            int changeCount = await db.SaveChangesAsync();
            if (changeCount == shouldChangeCount)
            {
                //  删除原头像文件
                Files.File.Delete(sourceAvatar.SaveName);
                //  删除缩略图
                Files.File.DeleteThumbnail(sourceAvatar.Thumbnail);
                //  缓存用户更新后的数据
                UserCache.SetUserModel(user);
                return (true, "");
            }
            throw new Exception("修改头像失败");
        }
    }
}
