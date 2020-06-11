using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;
using DB;

namespace Domain.Blogs
{
    public class Blog : EntityBase
    {
        public enum BlogState
        {
            [Description("启用")]
            Enabled,
            [Description("禁用")]
            Disabled,
            [Description("草稿")]
            Draft,
            [Description("待审核")]
            Audit
        }

        private readonly DB.Tables.Blog _blog;
        internal Blog(DB.Tables.Blog model)
        {
            if (model is null)
                throw new NullReferenceException();
            Id = model.Id;
            _blog = model;
        }

        public async Task<Results.BlogDetail> GetDetailAsync(bool readed = false)
        {
            DiagnosisNull(_blog);

            _blog.Views++;
            var author = _blog.Author;
            _blog.Author = null;

            await using var db = new LOPDbContext();
            db.Blogs.Update(_blog);
            int changeConut = await db.SaveChangesAsync();
            if (changeConut != 1)
                throw new Exception("增加已读人数出现错误");
            _blog.Author = author;

            int[] targetIds = _blog.TargetIds.SplitToInt(',').ToArray();
            Targets targets = new Targets();
            var targetNames = await targets.GetTargetsNameAsync(targetIds);

            Results.BlogDetail result = new Results.BlogDetail
            {
                Title = _blog.Title,
                Targets = targetNames,
                Content = _blog.Content,
                Views = _blog.Views,
                Likes = _blog.Likes,
                Author = _blog.Author.Name,
                AuthorAccount = _blog.Author.Account,
                DateTime = _blog.CreateDate.ToString("yyyy/MM/dd HH:mm")
            };
            return result;
        }

        /// <summary>
        /// 修改博文
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task ModifyAsync(Models.ModifyPost model)
        {
            var targetIds = await Targets.AppendTargetsAsync(model.Targets);

            _blog.Title = model.Title;
            _blog.TargetIds = string.Join(',', targetIds);
            _blog.Content = model.Content;
            if (_blog.State == (int)BlogState.Disabled)
                _blog.State = (int)BlogState.Audit;

            var author = _blog.Author;
            _blog.Author = null;

            await using var db = new LOPDbContext();
            db.Blogs.Update(_blog);
            int changeConut = await db.SaveChangesAsync();
            if (changeConut != 1)
                throw new Exception("修改博文出现错误");
            _blog.Author = author;
        }
        
        /// <summary>
        /// 点赞
        /// </summary>
        /// <returns></returns>
        public async Task LikeAsync()
        {
            _blog.Likes++;
            var author = _blog.Author;
            _blog.Author = null;

            await using var db = new LOPDbContext();
            db.Blogs.Update(_blog);
            int changeConut = await db.SaveChangesAsync();
            if (changeConut != 1)
                throw new Exception("点赞发生错误");
            _blog.Author = author;
        }
    }
}
