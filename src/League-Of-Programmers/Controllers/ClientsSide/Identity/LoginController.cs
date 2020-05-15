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

namespace League_Of_Programmers.Controllers.Clients.Identity
{
    public class LoginController : ClientsSideController
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }

        [HttpPatch]
        public async Task<IActionResult> IndexAsync([FromBody]Domain.Users.Models.Login model)
        {
            Domain.Users.UserManager userManager = new Domain.Users.UserManager();
            (var user, string msg) = await userManager.LoginAsync(model);
            if (user is null)
                return BadRequest(ModelState.AddMessageError(msg));

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
