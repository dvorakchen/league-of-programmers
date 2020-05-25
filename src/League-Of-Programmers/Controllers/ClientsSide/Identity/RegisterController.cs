using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using League_Of_Programmers.Controllers.Clients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace League_Of_Programmers.Controllers.ClientsSide.Identity
{
    [ApiController]
    public class RegisterController : ClientsSideController
    {
        /// <summary>
        /// register a new account
        /// </summary>
        // /api/clients/register
        [HttpPost]
        public async Task<IActionResult> IndexAsync([FromBody]Domain.Users.Models.Register model)
        {
            Domain.Users.UserManager userManager = new Domain.Users.UserManager();
            (bool isSuccess, string msg) = await userManager.RegisterAsync(model);
            if (isSuccess)
                return Created("", null);
            return BadRequest(msg);
        }
    }
}
