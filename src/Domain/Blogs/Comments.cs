using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DB;

namespace Domain.Blogs
{
    /// <summary>
    /// 评论
    /// </summary>
    internal class Comments
    {


        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="blogId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public async Task AddComment(int blogId, string content, int? commenter)
        {
            await using var db = new LOPDbContext();
            DB.Tables.Comment newComment = new DB.Tables.Comment
            { 
                BlogId = blogId,
                Content = content,
                CommenterId = commenter
            };
            db.Comments.Add(newComment);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount != 0)
                throw new Exception("评论失败");
        }
    }
}
