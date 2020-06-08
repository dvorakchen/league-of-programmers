using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DB;
using Common;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Domain.Users
{
    /// <summary>
    /// 客户
    /// </summary>
    public class Client : User
    {
        internal Client(DB.Tables.User userModel) : base(userModel)
        {
        }

        /// <summary>
        /// 获取客户首页的个人信息
        /// </summary>
        /// <returns></returns>
        public async Task<Results.ClientHomePageProfile> GetProfileAsync()
        {
            await using var db = new LOPDbContext();
            DB.Tables.User user = await db.Users.AsNoTracking().Include(user => user.Avatar).FirstOrDefaultAsync(user => user.Id == Id);
            if (user is null)
                return null;
            Results.ClientHomePageProfile result = new Results.ClientHomePageProfile
            {
                UserName = user.Account,
                Email = user.Email,
                Avatar = Path.Combine(Config.GetValue("File:SaveWebPath"), user.Avatar.SaveName)
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

        /// <summary>
        /// post new blog
        /// </summary>
        /// <param name="model"></param>
        /// <returns>(new post id, new post url title)</returns>
        public async Task<(int, string)> WriteBlogAsync(Blogs.Models.NewPost model)
        {
            Blogs.BlogsManager blogsManager = new Blogs.BlogsManager();
            int id = await blogsManager.WriteBlogAsync(model);

            if (id == Blogs.BlogsManager.POST_DEFEATED)
                return (Blogs.BlogsManager.POST_DEFEATED, "");
            return (id, model.Title.Replace(' ', '-'));
        }
    }
}
