using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

namespace League_Of_Programmers.Controllers.Clients.Identity
{
    public class LogoutController : ClientsSideController
    {
        
        public LogoutController() { }

        /*
         * return: 
         *  -   200: logout successfully
         */
        //  patch: /api/clients/login
        [HttpPatch, Authorize]
        public IActionResult IndexAsync()
        {
            Response.Cookies.Delete(JWT_KEY);
            return Ok();
        }

    }
}
