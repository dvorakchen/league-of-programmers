using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DB;
using Common;
using System.IO;

namespace Domain.Users
{
    /// <summary>
    /// 客户
    /// </summary>
    public class Client : User
    {
        internal Client(DB.Tables.User userModel): base(userModel)
        {
        }

        /// <summary>
        /// 获取客户首页的个人信息
        /// </summary>
        /// <returns></returns>
        public async Task<Results.ClientHomePageProfile> GetProfileAsync()
        {
            var user = await UserCache.GetUserModelAsync(Id, true);
            if (user is null)
                return null;
            Results.ClientHomePageProfile result = new Results.ClientHomePageProfile
            { 
                Account = user.Account,
                Email = user.Email,
                Avatar = Path.Combine(Config.GetValue(""), user.Avatar.SaveName)
            };
            return result;
        }

        /// <summary>
        /// 获取客户的博文
        /// </summary>
        /// <returns></returns>
        public async Task<Results.Blogs> GetBlogsAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Client: <管理员名>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Client)}: " + base.ToString();
        }
    }
}
