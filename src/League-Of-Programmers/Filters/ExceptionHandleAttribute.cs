using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace League_Of_Programmers.Filters
{
    public class ExceptionHandleAttribute : IExceptionFilter
    {
        private readonly ILogger logger;
        private readonly IWebHostEnvironment hostEnvironment;

        public ExceptionHandleAttribute(
            ILogger<ExceptionHandleAttribute> logger,
            IWebHostEnvironment hostEnvironment
            )
        {
            this.logger = logger;
            this.hostEnvironment = hostEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            if (!hostEnvironment.IsDevelopment())
            {
                //  非开发环境下
                return;
            }
            logger.LogError(context.Exception, " \r\nin router: {0}", context.HttpContext.Request.Path);
        }
    }
}
