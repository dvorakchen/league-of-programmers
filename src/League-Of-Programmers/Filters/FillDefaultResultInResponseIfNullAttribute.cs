using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace League_Of_Programmers.Filters
{
    /// <summary>
    /// 如果返回的结果是空值，null，就填充进一个空对象
    /// </summary>
    public class FillDefaultResultInResponseIfNullAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;
            if (result is ObjectResult obj)
            {
                if (obj.Value == null || (obj.Value is string s && string.IsNullOrWhiteSpace(s)))
                    obj.Value = new { };
            }
        }
    }
}
