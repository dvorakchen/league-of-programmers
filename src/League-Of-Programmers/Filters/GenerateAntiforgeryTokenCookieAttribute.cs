using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace League_Of_Programmers.Filters
{
    /// <summary>
    /// 生成防伪令牌
    /// </summary>
    public class GenerateAntiforgeryTokenCookieAttribute : ResultFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var antiforgery = context.HttpContext.RequestServices.GetService(typeof(IAntiforgery)) as IAntiforgery;

            // Send the request token as a JavaScript-readable cookie
            var tokens = antiforgery.GetAndStoreTokens(context.HttpContext);

            context.HttpContext.Response.Cookies.Append(
                "RequestVerificationToken",
                tokens.RequestToken,
                new CookieOptions() { HttpOnly = false });
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}
