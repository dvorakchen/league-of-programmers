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
        internal async Task<User> GetUserAsync(int id)
        {
            await using var db = new LOPDbContext();
            var userModel = await db.Users.FirstOrDefaultAsync(user => user.Id == id);
            return userModel == null ? null : User.Parse(userModel);
        }

        /// <summary>
        /// get client by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Client> GetClientAsync(int id)
        {
            var user = await GetUserAsync(id);
            if (user is null)
                throw new Exception($"{user} 不是客户");
            //  管理员也是客户
            return ParseClient(user);
        }

        /// <summary>
        /// get administrator by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Administrator> GetAdministratorAsync(int id)
        {
            var user = await GetUserAsync(id);
            if ((user.Role & User.RoleCategories.Administrator) != 0)
                return user as Administrator;
            throw new Exception($"{user} 不是管理员");
        }

        public async Task<Client> GetClientAsync(string account)
        {
            var clientModel = await UserCache.GetUserModelAsync(account);
            if (clientModel is null)
                return null;
            User user = User.Parse(clientModel);
            //  管理员也是客户
            return ParseClient(user);
        }

        private Client ParseClient(User user)
        {
            return user.Role switch
            {
                //  如果是管理员
                var r when (r & User.RoleCategories.Administrator) != 0 => (Client)(user as Administrator),
                //  如果是客户
                var r when (r & User.RoleCategories.Client) != 0 => user as Client,
                _ => throw new Exception($"{user} 不是客户")
            };
        }

        public async Task<Administrator> GetAdministratorAsync(string account)
        {
            return User.Parse(await UserCache.GetUserModelAsync(account)) as Administrator;
        }

        /// <summary>
        /// 是否有这个用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns>(是否有这个用户，用户名)</returns>
        public async Task<bool> HasUserAsync(string account)
        {
            await using var db = new LOPDbContext();
            return await db.Users.AnyAsync(user => user.Account.Equals(account, StringComparison.OrdinalIgnoreCase));
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
            if (!validation.ValidateAccount(model.Account) || model.Account.Contains(' '))
                return (false, $"注册账号长度不能大于{User.ACCOUNT_MAX_LENGTH}位小于{User.ACCOUNT_MIN_LENGTH}位且不能有空格");
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
