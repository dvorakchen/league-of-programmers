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

        public async Task<Paginator> GetBlogListAsync(Paginator pager, int? state, string s)
        {
            Expression<Func<DB.Tables.Blog, bool>> whereStatement = b => true;
            if (state.HasValue)
                whereStatement = whereStatement.And(b => b.State == state);
            if (!string.IsNullOrWhiteSpace(s))
                whereStatement = whereStatement.And(b => b.Title.Contains(s));

            await using var db = new LOPDbContext();
            pager.TotalSize = await db.Blogs.CountAsync(whereStatement);
            var list = await db.Blogs.AsNoTracking()
                                     .Skip(pager.Skip)
                                     .Take(pager.Size)
                                     .Where(whereStatement)
                                     .Include(blog => blog.Author)
                                     .Select(blog => new Results.BlogItem
                                     { 
                                         Id = blog.Id,
                                         Title = blog.Title,
                                         DateTime = blog.CreateDate.ToString("yyyy/MM/dd HH:mm"),
                                         Views = blog.Views,
                                         State = KeyValuePair.Create(blog.State, blog.State.GetDescription<Blog.BlogState>())
                                     })
                                     .ToListAsync();
            pager.List = list;
            return pager;
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
    }
}
