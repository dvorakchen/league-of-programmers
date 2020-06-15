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
         *      
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
    }
}
