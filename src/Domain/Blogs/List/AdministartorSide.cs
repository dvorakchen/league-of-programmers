using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Blogs.List
{
    internal class AdministartorSide : IBlogList
    {
        public async Task<Paginator> GetListAsync(Paginator pager)
        {
            Expression<Func<DB.Tables.Blog, bool>> whereStatement = blog => true;
            string s = pager["s"] ?? "";
            if (!string.IsNullOrWhiteSpace(s))
                whereStatement = whereStatement.And(blog => blog.Title.Contains(s));

            if (int.TryParse(pager["state"], out int state))
                whereStatement = whereStatement.And(blog => blog.State == state);

            await using var db = new LOPDbContext();
            pager.TotalSize = await db.Blogs.CountAsync(whereStatement);
            var list = await db.Blogs.AsNoTracking()
                                     .OrderByDescending(blog => blog.CreateDate)
                                     .Where(whereStatement)
                                     .Include(blog => blog.Author)
                                     .Skip(pager.Skip)
                                     .Take(pager.Size)
                                     .Select(blog => new Results.BlogItem
                                     {
                                         Id = blog.Id,
                                         Title = blog.Title,
                                         Author = blog.Author.Name,
                                         AuthorAccount = blog.Author.Account,
                                         DateTime = blog.CreateDate.ToString("yyyy/MM/dd HH:mm"),
                                         Views = blog.Views,
                                         Likes = blog.Likes,
                                         State = KeyValue.Create(blog.State, blog.State.GetDescription<Blog.BlogState>())
                                     })
                                     .ToListAsync();
            pager.List = list;
            return pager;
        }
    }
}
