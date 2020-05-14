/*
 * this middleware will set the cookie of antiforgery when the client sent request to the server
 * and make the anti forgery to header named X-XSRF-TOKEN
 * the antiforgery source code: https://github.com/myfor/AspNetCore/tree/master/src/Antiforgery
 */

using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace League_Of_Programmers.Middlewares
{
    public class Antiforgery
    {
        private readonly RequestDelegate _next;
        private readonly IAntiforgery _antiforgery;
        public Antiforgery(RequestDelegate _next, IAntiforgery _antiforgery)
        {
            this._next = _next;
            this._antiforgery = _antiforgery;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value;

            if (
                string.Equals(path, "/", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(path, "/index.html", StringComparison.OrdinalIgnoreCase))
            {
                // The request token can be sent as a JavaScript-readable cookie, 
                // and Angular uses it by default.
                var tokens = _antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken,
                    new CookieOptions() { HttpOnly = false });
            }
            else
            {
                if (context.Request.Cookies.TryGetValue("XSRF-TOKEN", out string antiForgery))
                {
                    context.Request.Headers.Add("X-XSRF-TOKEN", antiForgery);
                }
                else
                {
                    context.Request.Path = "/";
                }
            }

            await _next(context);
        }
    }
}
