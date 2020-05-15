using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Users
{
    public class UserManager : Info, IUserManager
    {
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
            return (User.Parse(userModel), NOT_MESSAGE);
        }
    }
}
