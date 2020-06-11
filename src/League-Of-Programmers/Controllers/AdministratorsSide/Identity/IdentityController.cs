using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Users;
using League_Of_Programmers.Controllers.Clients;
using Microsoft.AspNetCore.Mvc;

namespace League_Of_Programmers.Controllers.AdministratorsSide.Identity
{
    public class IdentityController : AdministratorsSideController
    {
        private readonly IUserManager userManager;

        public IdentityController(IUserManager _userManager)
        {
            userManager = _userManager;
        }

        /*
         *  is current user logged In and is administrator
         *  return:
         *      200:    yes
         *      401:    no
         */
        [HttpGet("check")]
        public async Task<IActionResult> CheckAsync()
        {
            bool user = await userManager.HasUserAsync(CurrentUserAccount);
            if (user)
            {
                var administrator = await userManager.GetAdministratorAsync(CurrentUserId);
                //  返回登录账号，用户名，角色
                Results.LoginResult result = new Results.LoginResult
                {
                    Account = administrator.Account,
                    UserName = administrator.Name,
                    Role = (int)administrator.Role
                };
                return Ok(result);
            }
            return Unauthorized();
        }
    }
}
