using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace League_Of_Programmers.Filters
{
    /// <summary>
    /// 如果返回的结果是字符串，就修改为 JSON 能够解析的字符串
    /// </summary>
    public class ChangeResultStringToJsonStringAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;
            if (result is ObjectResult obj)
            {
                if (obj.Value is string s && !string.IsNullOrWhiteSpace(s))
                    obj.Value = $"\"{s}\"";
            }
            
        }
    }
}
