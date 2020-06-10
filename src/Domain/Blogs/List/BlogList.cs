using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Blogs.List
{
    internal class BlogList : IBlogList
    {
        private const int SEARCH_MAX_LENGTH = 256;
        public async Task<Paginator> GetListAsync(Paginator pager)
        {
            string s = pager["s"] ?? "";
            if (s.Length > SEARCH_MAX_LENGTH)
                s = s.Substring(0, SEARCH_MAX_LENGTH);

            Expression<Func<DB.Tables.Blog, bool>> whereStatement = blog => blog.State == (int)Blog.BlogState.Enabled;
            var searchStatement = WhereExpression(s);
            if (searchStatement != null)
                whereStatement = whereStatement.And(searchStatement);

            await using var db = new LOPDbContext();
            pager.TotalSize = await db.Blogs.CountAsync(whereStatement);
            pager.List = await db.Blogs.AsNoTracking()
                                       .OrderByDescending(blog => blog.Likes)
                                       .ThenByDescending(blog => blog.CreateDate)
                                       .Where(whereStatement)
                                       .Skip(pager.Skip)
                                       .Take(pager.Size)
                                       .Include(blog => blog.Author)
                                       .Select(blog => new Results.BlogItem 
                                       {
                                           Id = blog.Id,
                                           Title = blog.Title,
                                           Author = blog.Author.Name,
                                           AuthorAccount = blog.Author.Account,
                                           DateTime = blog.CreateDate.ToString("yyyy/MM/dd HH:mm"),
                                           Views = blog.Views,
                                           Likes = blog.Likes,
                                           State = KeyValuePair.Create(blog.State, blog.State.GetDescription<Blog.BlogState>())
                                       })
                                       .ToListAsync();
            return pager;
        }

        /// <summary>
        /// 根据搜索字符拼接查询表达式树
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private Expression<Func<DB.Tables.Blog, bool>> WhereExpression(string search)
        {
            string[] searchKeywords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Expression<Func<DB.Tables.Blog, bool>> where = q => false;
            if (searchKeywords.Length == 0)
                where = null;
            else
            {
                for (int i = 0; i < searchKeywords.Length; i++)
                {
                    string keyword = searchKeywords[i];
                    if (i == 0)
                        where = q => q.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase);
                    else
                        where = where.Or(q => q.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase));
                }
            }
            return where;
        }
    }
}
