using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class UserManager : IUserManager
    {
        /// <summary>
        /// get user by user id
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns>user or null if not exist</returns>
        public async Task<User> GetUser(int id)
        {
            var userModel = await UserCache.GetUserModelAsync(id);
            return userModel == null ? null : User.Parse(userModel);
        }

        /// <summary>
        /// user login
        /// </summary>
        /// <param name="model"></param>
        /// <returns>(登录后的用户，登录失败的原因)</returns>
        public async Task<(User, string)> LoginAsync(Models.Login model)
        {
            if (string.IsNullOrWhiteSpace(model.Account))
                return (null, "登录账号不能为空");
            if (string.IsNullOrWhiteSpace(model.Password))
                return (null, "密码不能为空");

            await using var db = new LOPDbContext();
            var userModel = await db.Users.FirstOrDefaultAsync(user =>
                user.Account.Equals(model.Account, StringComparison.OrdinalIgnoreCase) && user.Password.Equals(model.Password, StringComparison.OrdinalIgnoreCase));
            if (userModel is null)
                return (null, "账号不存在或密码不正确");
            else
                return (User.Parse(userModel), "");
        }

        public async Task<(bool, string)> RegisterAsync(Models.Register model)
        {
            if (model.Account.Length < User.ACCOUNT_MIN_LENGTH)
                return (false, $"注册账号长度不能小于{User.ACCOUNT_MIN_LENGTH}位");
            if (string.IsNullOrWhiteSpace(model.Password))
                return (false, $"密码不能为空");
            if (model.Password != model.ConfirmPassword)
                return (false, $"两次密码不一致");

            await using var db = new LOPDbContext();
            if (await db.Users.AnyAsync(user => user.Account.Equals(model.Account, StringComparison.OrdinalIgnoreCase)))
                return (false, $"账号已经被使用");

            DB.Tables.User newUser = new DB.Tables.User
            {
                Account = model.Account,
                Password = model.Password
            };
            db.Users.Add(newUser);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return (true, "");
            else
                return (false, "注册失败，请重试");
        }
    }
}
