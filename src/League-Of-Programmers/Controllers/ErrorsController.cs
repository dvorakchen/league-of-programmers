using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace League_Of_Programmers.Controllers
{
    
    public class ErrorsController : LOPController
    {
        /// <summary>
        /// 测试用，用于出发异常
        /// </summary>
        [Conditional("DEBUG")]
        [HttpGet]
        public void Index()
        {
            throw new NotImplementedException();
        }
    }
}
