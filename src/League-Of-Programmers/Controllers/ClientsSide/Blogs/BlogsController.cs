using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Users;
using Domain.Blogs;
using League_Of_Programmers.Controllers.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace League_Of_Programmers.Controllers.ClientsSide.Blogs
{
    public class BlogsController : ClientsSideController
    {
        private readonly IUserManager _userManager;
        public BlogsController(IUserManager _userManager)
        {
            this._userManager = _userManager;
        }

        /*
         *  get blogs list
         *  
         *  /api/clients/blogs
         *  
         *  return: 
         *      200:    successfully
         *      
         */
        [HttpGet]
        public async Task<IActionResult> GetBlogsAsync()
        {
            throw new NotImplementedException();
        }

        /*
         *  post new blog
         * 
         *  /api/clients/blogs
         *  
         *  return:
         *      201:    successfully
         *      400:    parameters fault
         *      202:    fault
         */
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostNewBlogAsync([FromBody]Domain.Blogs.Models.NewPost model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
                return BadRequest("标题不能为空");
            if (model.Targets.Length == 0)
                return BadRequest("标签不能为空");
            if (string.IsNullOrWhiteSpace(model.Content))
                return BadRequest("内容不能为空");

            model.AuthorId = CurrentUserId;

            var client = await _userManager.GetClientAsync(CurrentUserId);

            (int id, string title) = await client.WriteBlogAsync(model);
            if (id == BlogsManager.POST_DEFEATED)
                return Accepted();

            string url = $"/blogs/{id}/{title}";
            return Created(url, url);
        }

        /*
         *  modify blog
         *  
         *  /api/clients/blogs/{id}
         *  
         *  return:
         *      200:    successfully
         *      400:    parameter fault
         */
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> ModifyBlogAsync(int id, [FromBody]Domain.Blogs.Models.ModifyPost model)
        {
            model.Id = id;
            model.AuthorId = CurrentUserId;


            throw new NotImplementedException();
        }
    }
}
