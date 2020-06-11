using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace League_Of_Programmers.Controllers
{
    /// <summary>
    /// 控制器的基类
    /// </summary>
    [ApiController]
    //  [AutoValidateAntiforgeryToken]
    [Route("/api")]
    public abstract class LOPController : ControllerBase 
    {
        public const string JWT_KEY = "_j_w_t_";

        protected const int NOT_ID = -1;
        /// <summary>
        /// 当前登录用户的ID,
        /// 如果获取不到，会返回常量 NOT_ID
        /// </summary>
        protected int CurrentUserId
        {
            get
            {
                var idClaim = User.FindFirstValue(ClaimTypes.PrimarySid);
                if (int.TryParse(idClaim, out int userId))
                    return userId;
                return NOT_ID;
            }
        }

        /// <summary>
        /// 当前登录用户的 账号,
        /// 如果获取不到，null
        /// </summary>
        protected string CurrentUserAccount
        {
            get
            {
                var account = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return account;
            }
        }
    }
}
