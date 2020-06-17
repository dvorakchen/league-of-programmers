using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Blogs;
using League_Of_Programmers.Controllers.Clients;
using Microsoft.AspNetCore.Mvc;

namespace League_Of_Programmers.Controllers.AdministratorsSide.Blogs
{
    public class BlogsController : AdministratorsSideController
    {
        /*
         *  get blogs list in administrator side
         *  
         *  /api/administrators/blogs
         *  
         *  return: 
         *      200:    successfully
         */
        [HttpGet]
        public async Task<IActionResult> GetBlogsAsync(int index, int size, string s, int? state)
        {
            var pager = Domain.Paginator.New(index, size, 2);
            pager["s"] = s ?? "";
            pager["state"] = state.HasValue ? state.Value.ToString() : "";

            BlogsManager blogsManager = new BlogsManager();
            pager = await blogsManager.GetBlogListAsync(BlogsManager.ListType.AdministartorSide, pager);
            return Ok(pager);
        }

        /*
         *  get blog detail in administrator side
         */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBlogDetailAsync(int id)
        {
            var blog = await new BlogsManager().GetBlogAsync(id);
            if (blog == null)
                return NotFound();
            var detail = await blog.GetDetailAsync();
            return Ok(detail);
        }

        /*
         *  启用博文
         *  
         *  /api/administrator/blogs/{id}/enable
         *  
         *  return:
         *      204:    successfully
         *      400:    fault
         */
        [HttpPatch("{id}/enable")]
        public async Task<IActionResult> EnableAsync(int id)
        {
            var blog = await new BlogsManager().GetBlogAsync(id);
            if (blog == null)
                return BadRequest("该博文已不存在");
            (bool isSucc, string msg) = await blog.EnableAsync();
            if (isSucc)
                return NoContent();
            return BadRequest(msg);
        }

        /*
         *  禁用博文
         *  
         *  /api/administrator/blogs/{id}/disable
         *  
         *  return:
         *      204:    successfully
         *      400:    fault
         */
        [HttpPatch("{id}/disable")]
        public async Task<IActionResult> DisableAsync(int id)
        {
            var blog = await new BlogsManager().GetBlogAsync(id);
            if (blog == null)
                return BadRequest("该博文已不存在");
            (bool isSucc, string msg) = await blog.DisableAsync();
            if (isSucc)
                return NoContent();
            return BadRequest(msg);
        }
    }
}
