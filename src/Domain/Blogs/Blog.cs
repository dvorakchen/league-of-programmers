using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Domain.Blogs
{
    public class Blog : EntityBase
    {
        private readonly DB.Tables.Blog _blog;
        internal Blog(DB.Tables.Blog model)
        {
            if (model is null)
                throw new NullReferenceException();
            Id = model.Id;
            _blog = model;
        }

        public async Task<Results.BlogDetail> GetDetailAsync()
        {
            DiagnosisNull(_blog);

            int[] targetIds = _blog.TargetIds.SplitToInt(',').ToArray();
            Targets targets = new Targets();
            var targetNames = await targets.GetTargetsNameAsync(targetIds);

            Results.BlogDetail result = new Results.BlogDetail
            {
                Title = _blog.Title,
                Targets = targetNames,
                Content = _blog.Content,
                Views = _blog.Views,
                Author = _blog.Author.Name,
                AuthorAccount = _blog.Author.Account,
                DateTime = _blog.CreateDate.ToString("yyyy/MM/dd HH:mm")
            };
            return result;
        }
    }
}
