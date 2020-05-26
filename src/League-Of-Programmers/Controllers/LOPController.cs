using Microsoft.AspNetCore.Mvc;

namespace League_Of_Programmers.Controllers
{
    /// <summary>
    /// 控制器的基类
    /// </summary>
    [ApiController]
    //[ValidateAntiForgeryToken]
    [AutoValidateAntiforgeryToken]
    [Route("api/[controller]")]
    public abstract class LOPController : ControllerBase
    {

    }
}
