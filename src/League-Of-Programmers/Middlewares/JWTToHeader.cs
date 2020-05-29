using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace League_Of_Programmers.Middlewares
{
    /// <summary>
    /// 将 cookie 中的 JWT 放到 header 上
    /// </summary>
    public class JWTToHeader
    {
        private readonly RequestDelegate _next;

        public JWTToHeader(RequestDelegate _next)
        {
            this._next = _next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue(Controllers.LOPController.JWT_KEY, out string jwt))
            {
                context.Request.Headers.Add("Authorization", $"Bearer {jwt}");
            }

            await _next(context);

        }
    }
}
