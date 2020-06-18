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
    public class ClientsController : ClientsSideController
    {
        private readonly IUserManager _userManager;
        public ClientsController(IUserManager _userManager)
        {
            this._userManager = _userManager;
        }

        /*
         *  检查当前用户
         *  
         *  /api/clients/user/{account}/check
         *  
         *  return:
         *      200:    has user
         *      404:    has not client
         */
        [HttpGet("{account}/check")]
        public async Task<IActionResult> CheckAsync(string account)
        {
            bool has = await _userManager.HasUserAsync(account);
            if (has)
                return Ok(account.Equals(CurrentUserAccount, StringComparison.OrdinalIgnoreCase));
            return NotFound();
        }

        /*
         *  获取用户主页信息
         *  
         *  /api/clients/user/{id}/home
         *  
         *  return:
         *      200:    successfully
         *      404:    client not exist
         */
        [HttpGet("{account}/home")]
        public async Task<IActionResult> GetClientInfoAsync(string account)
        {
            Client user = await _userManager.GetClientAsync(account);
            if (user is null)
                return NotFound();
            var profile = await user.GetProfileAsync();
            if (profile is null)
                return NotFound();
            return Ok(profile);
        }

        /*
         *  获取客户的博文
         *  
         *  /api/clients/user/{id}/blog
         *  
         *  return:
         *      200:    successfully
         *      404:    client not exist
         */
        [HttpGet("{account}/blogs")]
        public async Task<IActionResult> GetClientBlogAsync(int index, int size, int? state, string account, string s)
        {
            var client = await _userManager.GetClientAsync(account);
            if (client is null)
                return NotFound();
            var list = await client.GetBlogsAsync(index, size, state, s);
            return Ok(list);
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
        [Authorize]
        [HttpPatch("password")]
        public async Task<IActionResult> ModifyPasswordAsync([FromBody]string newPassword)
        {
            var currentUser = await _userManager.GetClientAsync(CurrentUserId);
            (bool isSuccessfully, string msg) = await currentUser.ModifyPasswordAsync(newPassword);
            if (isSuccessfully)
                return Ok();
            return BadRequest(msg);
        }

        /* 
         * 当前用户修改
         *  
         *  /api/clients/clients
         *
         *  return: 
         *      200:    successfully
         *      400:    defeated
         */
        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> ModifyAsync([FromBody] Models.ModifyUser model)
        {
            var currentUser = await _userManager.GetClientAsync(CurrentUserId);
            (bool isSuccessfully, string msg) = await currentUser.ModifyUser(model);
            if (isSuccessfully)
                return Ok();
            return BadRequest(msg);
        }

        /* 
         * 当前用户修改头像
         * 修改成功后会将新头像的访问链接返回
         *  
         *  /api/clients/users/avatar
         *
         *  return: 
         *      200:    successfully
         *      400:    defeated
         */
        [Authorize]
        [HttpPatch("avatar")]
        public async Task<IActionResult> ModifyAvatarAsync([FromBody]int avatarId)
        {
            var currentUser = await _userManager.GetClientAsync(CurrentUserId);
            (bool isSuccessfully, string msg) = await currentUser.ModifyAvatarAsync(avatarId);
            if (isSuccessfully)
                return Ok(msg);
            return BadRequest(msg);
        }
    }
}
