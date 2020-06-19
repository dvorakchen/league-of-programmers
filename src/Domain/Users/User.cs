/*
 *  用户
 *  
 *  用户既可以是客户，也可以是管理员
 *  所以这个 User 类应该是客户和管理员的抽象类
 *  
 *  Role:   用户的角色, 不可修改
 *  
 */

using Common;
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Domain.Users
{
    public abstract class User : EntityBase, IEquatable<User>
    {
        public const int ACCOUNT_MIN_LENGTH = 4;
        public const int ACCOUNT_MAX_LENGTH = 20;

        public const int NAME_MIN_LENGTH = 4;
        public const int NAME_MAX_LENGTH = 20;
        public const string NAME_NOT_ALLOW_CHAR = "\\*/'.,;=?";
        /// <summary>
        /// 单位 字节
        /// </summary>
        public const int AVATAR_MAX_BYTES_COUNT = 64 << 10;

        public const int AVATAR_DEFAULT_ID = 1;

        /// <summary>
        /// 角色分类
        /// </summary>
        [Flags]
        public enum RoleCategories
        {
            Client = 1 << 0,
            Administrator = 1 << 1,
        }
        protected User() { }
        protected User(DB.Tables.User userModel)
        {
            if (userModel is null)
                throw new ArgumentNullException("user model not found");
            Id = userModel.Id;
            Account = userModel.Account;
            Name = userModel.Name;
            Role = (RoleCategories)userModel.Roles;

            UserCache.SetUserModel(userModel);
        }
        /// <summary>
        /// use account
        /// </summary>
        public string Account { get; protected set; }
        /// <summary>
        /// user name
        /// </summary>
        public string Name { get; protected set; }
        /// <summary>
        /// 角色
        /// </summary>
        public RoleCategories Role { get; protected set; }
        /// <summary>
        /// return user name
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;
        /// <summary>
        /// user's hash code is Id
        /// </summary>
        public override int GetHashCode() => Id;
        public override bool Equals(object obj)
        {
            if (obj is User u)
                return Equals(u);
            return false;
        }
        public bool Equals(User other)
        {
            return other.Id == Id;
        }

        /// <summary>
        /// 获取客户首页的个人信息
        /// </summary>
        /// <returns></returns>
        public async virtual Task<Results.ClientHomePageProfile> GetProfileAsync()
        {
            DB.Tables.User user = await UserCache.GetUserModelAsync(Account, true);
            if (user is null)
                return null;
            Results.ClientHomePageProfile result = new Results.ClientHomePageProfile
            {
                UserName = Name,
                Email = user.Email,
                Avatar = Path.Combine(Config.GetValue("File:SaveWebPath"), user.Avatar.SaveName)
            };
            return result;
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        public virtual async Task<Results.UserDetail> GetDetailAsync()
        {
            var userModel = await UserCache.GetUserModelAsync(Account, true);
            if (userModel == null)
                return null;
            var result = new Results.UserDetail
            { 
                UserName = Name,
                Account = Account,
                Avatar = Path.Combine(Config.GetValue("File:SaveWebPath"), userModel.Avatar.SaveName),
                Email = userModel.Email,
                CreateDate = userModel.CreateDate.ToString("yyyy/MM/dd HH:mm")
            };
            return result;
        }

        public virtual async Task<(bool, string)> ModifyUser(Models.ModifyUser model)
        {
            Validation validation = new Validation();
            if (!validation.ValidateUserName(model.Name))
                return (false, $"用户名必须大于{NAME_MIN_LENGTH}位小于{NAME_MAX_LENGTH}位，却不能带有 {NAME_NOT_ALLOW_CHAR}");
            if (!validation.ValidateEmail(model.Email))
                return (false, "邮箱格式不正确");

            await using var db = new LOPDbContext();
            DB.Tables.User user = await db.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == Id);
            if (user is null)
                return (false, "该用户不存在");

            if (user.Name == model.Name && user.Email == model.Email)
                return (true, "");
            user.Name = model.Name;
            user.Email = model.Email;
            db.Update(user);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
            {
                UserCache.SetUserModel(user);
                Name = model.Name;
                return (true, "");
            }
            throw new Exception("修改失败");
        }

        /// <summary>
        /// modify the user email
        /// </summary>
        /// <returns></returns>
        public virtual async Task<(bool, string)> ModifyPasswordAsync(Models.ChangePassword model)
        {
            await using var db = new LOPDbContext();
            DB.Tables.User user = await db.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == Id && user.Password.Equals(model.OldPassword, StringComparison.OrdinalIgnoreCase));
            if (user is null)
                return (false, "旧密码错误");

            if (string.IsNullOrWhiteSpace(model.NewPassword))
                return (false, "密码不能为空");

            if (user.Password == model.NewPassword)
                return (true, "");
            user.Password = model.NewPassword;
            db.Update(user);
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
        /// <returns>(bool: isSuccessfully, string: message when fault or avatar request path when successfully)</returns>
        public virtual async Task<(bool, string)> ModifyAvatarAsync(int avatarId)
        {
            await using var db = new LOPDbContext();
            DB.Tables.User user = await db.Users.AsNoTracking().Include(user => user.Avatar).FirstOrDefaultAsync(user => user.Id == Id);
            if (user is null)
                return (false, "该用户不存在");
            if (user.AvatarId == avatarId)
                return (true, "");
            
            DB.Tables.File SOURCE_AVATAR = user.Avatar;
            int shouldChangeCount = 2;
            //  删除原头像
            if (user.AvatarId != AVATAR_DEFAULT_ID && user.Avatar != null)
            {
                db.Files.Remove(SOURCE_AVATAR);
                shouldChangeCount++;
            }

            //  user.AvatarId = avatarId;
            var avatarModel = await db.Files.AsNoTracking().FirstOrDefaultAsync(file => file.Id == avatarId);
            if (avatarModel == null)
                return (false, "该头像不存在");

            user.Avatar = avatarModel;
            db.Users.Update(user);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == shouldChangeCount)
            {
                if (SOURCE_AVATAR.Id != AVATAR_DEFAULT_ID)
                {
                    //  删除原头像文件
                    Files.File.Delete(SOURCE_AVATAR.SaveName);
                    //  删除缩略图
                    Files.File.DeleteThumbnail(SOURCE_AVATAR.Thumbnail);
                }
                //  缓存用户更新后的数据
                UserCache.SetUserModel(user);
                //  返回新头像的访问路径
                string saveWebPath = Config.GetValue("File:SaveWebPath");
                saveWebPath = Path.Combine(saveWebPath, avatarModel.SaveName);
                return (true, saveWebPath);
            }
            throw new Exception("修改头像失败");
        }
    }
}
