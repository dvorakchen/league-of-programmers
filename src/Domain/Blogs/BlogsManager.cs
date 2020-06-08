using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DB;

namespace Domain.Blogs
{
    /// <summary>
    /// 博文管理
    /// </summary>
    public class BlogsManager
    {
        public const int POST_DEFEATED = -1;


        public async Task<int> WriteBlogAsync(Models.NewPost model)
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
