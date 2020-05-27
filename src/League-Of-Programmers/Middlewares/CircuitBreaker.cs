/*
 * circuit breaker
 * 当客户端频繁访问同一个接口时，会触发熔断机制，禁止客户在一段时间内再次访问接口, 返回 429 状态码
 * 在配置文件中配置熔断参数：
 * "CricuitBreaker": {
 *   // 触发熔断的次数
 *   "LimitCount": 7,
 *   //  熔断检测的时间范围，单位 秒
 *   "TimeScopes": 60
 * }
 */

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace League_Of_Programmers.Middlewares
{
    /// <summary>
    /// 熔断
    /// </summary>
    public class CircuitBreaker
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        public CircuitBreaker(RequestDelegate _next, IMemoryCache _cache, IConfiguration _configuration)
        {
            this._cache = _cache;
            this._next = _next;
            this._configuration = _configuration;
        }

        /// <summary>
        /// 熔断
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var CricuitBreaker = _configuration.GetSection("CricuitBreaker");
            //  熔断触发次数
            var limitCount = CricuitBreaker.GetValue<int>("LimitCount");

            var path = context.Request.Path;
            var clientAddress = context.Connection.RemoteIpAddress.ToString();
            var key = string.Concat(clientAddress, path);

            _ = _cache.TryGetValue(key, out byte currentCount);

            if (currentCount < limitCount)
            {
                double timeScopes = CricuitBreaker.GetValue<double>("TimeScopes");
                //_cache.Set(key, ++currentCount, DateTimeOffset.Now.AddSeconds(timeScopes));
                _cache.Set(key, ++currentCount, new MemoryCacheEntryOptions
                { 
                    Size = sizeof(byte),
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(timeScopes)
                });
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = 429;
                context.Response.ContentType = "text/plain";
                context.Response.ContentLength = 0;
            }
        }
    }
}
