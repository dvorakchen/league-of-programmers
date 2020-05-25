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
    public class LoginController : ClientsSideController
    {
        private readonly IConfiguration _configuration;
        ILogger _logger;
        public LoginController(IConfiguration _configuration, ILogger<LoginController> _logger)
        {
            this._configuration = _configuration;
            this._logger = _logger;
        }

        /*
         * return: 
         *  -   200: login success, the token in body
         *  -   400: login fault, the reason in body
         */
        //  patch: /api/clients/login
        [HttpPatch]
        public async Task<IActionResult> IndexAsync([FromBody]Domain.Users.Models.Login model)
        {
            Domain.Users.UserManager userManager = new Domain.Users.UserManager();
            (var user, string msg) = await userManager.LoginAsync(model);
            if (user is null)
                return BadRequest(msg);

            //  build JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name)
            };

            string secret = _configuration.GetSection("JwtSettings:Secret").Value;
            var accessExpiration = _configuration.GetSection("JwtSettings:AccessExpiration").Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                    _configuration.GetSection("JwtSettings:Issuer").Value,
                    _configuration.GetSection("JwtSettings:Audience").Value,
                    claims,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddMinutes(int.Parse(accessExpiration)),
                    credentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok(token);
        }

    }
}
