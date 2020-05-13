using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace League_Of_Programmers.Controllers
{
    /// <summary>
    /// 控制器的基类
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public abstract class LOPController : ControllerBase
    {

    }
}
