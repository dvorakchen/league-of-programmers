using System;
using System.Collections.Generic;
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
        public const int POST_DEFEATED = -1;

        public async Task<Blog> GetBlogAsync(int id)
        {
            await using var db = new LOPDbContext();
            var blogModel = await db.Blogs.AsNoTracking().FirstOrDefaultAsync(blog => blog.Id == id);
            if (blogModel is null)
                return null;
            return new Blog(blogModel);
        }

        public async Task<int> CreateBlogAsync(Models.NewPost model)
        {
            var targetIds = await Targets.AppendTargets(model.Targets);

            await using var db = new LOPDbContext();

            DB.Tables.Blog newBlog = new DB.Tables.Blog
            {
                Title = model.Title,
                Content = model.Content,
                TargetIds = string.Join(',', targetIds),
                AuthorId = model.AuthorId,
                IsDraft = model.IsDraft,
                CreateDate = DateTime.Now
            };
            db.Blogs.Add(newBlog);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return newBlog.Id;
            return POST_DEFEATED;
        }
    }
}
