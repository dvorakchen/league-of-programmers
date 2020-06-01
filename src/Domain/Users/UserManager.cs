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
        internal async Task<User> GetUser(int id)
        {
            var userModel = await UserCache.GetUserModelAsync(id);
            return userModel == null ? null : User.Parse(userModel);
        }

        /// <summary>
        /// get client by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Client> GetClient(int id)
        {
            var user = await GetUser(id);
            if ((user.Role & User.RoleCategories.Client) != 0)
                return user as Client;
            throw new Exception($"{user} 不是客户");
        }

        /// <summary>
        /// get client by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Administrator> GetAdministrator(int id)
        {
            var user = await GetUser(id);
            if ((user.Role & User.RoleCategories.Administrator) != 0)
                return user as Administrator;
            throw new Exception($"{user} 不是管理员");
        }

        /// <summary>
        /// 是否有这个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns>(是否有这个用户，用户名)</returns>
        public async Task<(bool, string)> HasUser(int id)
        {
            var user = await GetUser(id);
            if (user is null)
                return (false, "");
            return (true, user.Name);
        }

        /// <summary>
        /// user login，
        /// 客户或管理员都可以登录
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

        /// <summary>
        /// 注册客户，只能注册客户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> RegisterClientAsync(Models.Register model)
        {
            var validation = new Validation();
            if (!validation.ValidateAccount(model.Account))
                return (false, $"注册账号长度不能大于{User.ACCOUNT_MAX_LENGTH}位小于{User.ACCOUNT_MIN_LENGTH}位");
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
                Name = model.Account,
                Password = model.Password,
                Roles = (int)User.RoleCategories.Client
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
