using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Blogs
{
    /// <summary>
    /// 博文管理
    /// </summary>
    public class BlogsManager
    {
        public enum ListType
        {
            ClientDetailPage
        }

        public const int POST_DEFEATED = -1;

        public async Task<Blog> GetBlogAsync(int id)
        {
            await using var db = new LOPDbContext();
            var blogModel = await db.Blogs.AsNoTracking()
                                          .Include(b => b.Author)
                                          .FirstOrDefaultAsync(blog => blog.Id == id);
            if (blogModel is null)
                return null;
            return new Blog(blogModel);
        }

        public async Task<Paginator> GetBlogListAsync(ListType type, Paginator pager)
        {
            List.IBlogList blogList = type switch
            {
                ListType.ClientDetailPage => new List.ClientBlogs(),
                _ => throw new ArgumentException("未知的列表参数")
            };
            var list = await blogList.GetListAsync(pager);
            return list;
        }

        public async Task<int> CreateBlogAsync(Models.NewPost model)
        {
            var targetIds = await Targets.AppendTargetsAsync(model.Targets);

            await using var db = new LOPDbContext();

            DB.Tables.Blog newBlog = new DB.Tables.Blog
            {
                Title = model.Title,
                Content = model.Content,
                TargetIds = string.Join(',', targetIds),
                AuthorId = model.AuthorId,
                State = model.IsDraft ? (int)Blog.BlogState.Draft : (int)Blog.BlogState.Enabled,
                CreateDate = DateTime.Now
            };
            db.Blogs.Add(newBlog);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return newBlog.Id;
            return POST_DEFEATED;
        }

        /// <summary>
        /// 删除一个博文，只能删除自己的博文
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool, string)> DeleteBlogAsync(int id, int clientId)
        {
            await using var db = new LOPDbContext();
            var blog = await db.Blogs.AsNoTracking().FirstOrDefaultAsync(blog => blog.Id == id);
            if (blog is null)
                return (true, "");
            if (blog.AuthorId != clientId)
                return (false, "你没有权限删除这篇博文");
            db.Blogs.Remove(blog);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return (true, "");
            throw new Exception("删除博文失败");
        }
    }
}
