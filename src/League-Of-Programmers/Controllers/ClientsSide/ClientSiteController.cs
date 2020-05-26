using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace League_Of_Programmers.Controllers.Clients
{
    [Route("api/clients/[controller]")]
    public class ClientsSideController : LOPController
    {
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
    }
}
