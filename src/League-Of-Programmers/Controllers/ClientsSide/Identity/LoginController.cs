using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace League_Of_Programmers.Controllers.Clients.Identity
{
    public class LoginController : ClientsSideController
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        /*
         *  is current user logged In
         *  return:
         *      200:    yes
         *      401:    no
         */
        [HttpGet("check"), Authorize]
        public async Task<IActionResult> IsLoggedIn()
        {
            Domain.Users.UserManager userManager = new Domain.Users.UserManager();
            (bool user, string name) = await userManager.HasUser(CurrentUserId);
            if (user)
                return Ok(name);
            return Unauthorized();
        }

        /*
         * return: 
         *  -   200: login success, the user name in body
         *  -   400: login fault, the reason in body
         */
        //  patch: /api/clients/login
        [HttpPatch]
        public async Task<IActionResult> IndexAsync([FromBody] Domain.Users.Models.Login model)
        {
            Domain.Users.UserManager userManager = new Domain.Users.UserManager();
            (var user, string msg) = await userManager.LoginAsync(model);
            if (user is null)
                return BadRequest(msg);

            //  build JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.PrimarySid, user.Id.ToString())
            };

            string secret = _configuration.GetSection("JwtSettings:Secret").Value;
            var accessExpiration = _configuration.GetSection("JwtSettings:AccessExpiration").Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                    _configuration.GetSection("JwtSettings:Issuer").Value,
                    _configuration.GetSection("JwtSettings:Audience").Value,
                    claims,
                    notBefore: null,
                    DateTime.UtcNow.AddMinutes(int.Parse(accessExpiration)),
                    credentials
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            Response.Cookies.Append(JWT_KEY, token);
            //  返回登录用户名，角色
            Domain.Users.Results.LoginResult result = new Domain.Users.Results.LoginResult
            { 
                UserName = user.Name,
                Role = (int)user.Role
            };
            return Ok(result);
        }

    }
}
