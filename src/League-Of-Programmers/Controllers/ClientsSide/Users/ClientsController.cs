using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using League_Of_Programmers.Controllers.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Users;

namespace League_Of_Programmers.Controllers.ClientsSide.Users
{
    /// <summary>
    /// 当前用户，代表当前登录用户自己的操作
    /// </summary>
    [Authorize]
    public class ClientsController : ClientsSideController
    {
        private readonly IUserManager _userManager;
        public ClientsController(IUserManager _userManager)
        {
            this._userManager = _userManager;
        }

        /*
         * 当前用户修改密码
         *  
         *  /api/clients/users/password
         *
         *  return: 
         *      200:    successfully
         *      400:    defeated
         */
        [HttpPatch("password")]
        public async Task<IActionResult> ModifyPasswordAsync([FromBody]string newPassword)
        {
            var currentUser = await _userManager.GetClient(CurrentUserId);
            (bool isSuccessfully, string msg) = await currentUser.ModifyPasswordAsync(newPassword);
            if (isSuccessfully)
                return Ok();
            return BadRequest(msg);
        }

        /* 
         * 当前用户修改名字
         *  
         *  /api/clients/users/name
         *
         *  return: 
         *      200:    successfully
         *      400:    defeated
         */
        [HttpPatch("name")]
        public async Task<IActionResult> ModifyNameAsync([FromBody]string name)
        {
            var currentUser = await _userManager.GetClient(CurrentUserId);
            (bool isSuccessfully, string msg) = await currentUser.ModifyNameAsync(name);
            if (isSuccessfully)
                return Ok();
            return BadRequest(msg);
        }

        /* 
         * 当前用户修改邮箱
         *  
         *  /api/clients/users/email
         *
         *  return: 
         *      200:    successfully
         *      400:    defeated
         */
        [HttpPatch("email")]
        public async Task<IActionResult> ModifyEmailAsync([FromBody]string email)
        {
            var currentUser = await _userManager.GetClient(CurrentUserId);
            (bool isSuccessfully, string msg) = await currentUser.ModifyEmailAsync(email);
            if (isSuccessfully)
                return Ok();
            return BadRequest(msg);
        }

        /* 
         * 当前用户修改头像
         *  
         *  /api/clients/users/avatar
         *
         *  return: 
         *      200:    successfully
         *      400:    defeated
         */
        [HttpPatch("email")]
        public async Task<IActionResult> ModifyAvatarAsync([FromBody]int avatarId)
        {
            var currentUser = await _userManager.GetClient(CurrentUserId);
            (bool isSuccessfully, string msg) = await currentUser.ModifyAvatarAsync(avatarId);
            if (isSuccessfully)
                return Ok();
            return BadRequest(msg);
        }
    }
}
