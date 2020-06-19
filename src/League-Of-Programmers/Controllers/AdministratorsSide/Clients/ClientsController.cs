using Domain.Users;
using League_Of_Programmers.Controllers.Clients;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace League_Of_Programmers.Controllers.AdministratorsSide.Clients
{
    public class ClientsController : AdministratorsSideController
    {
        private readonly IUserManager _userManager;
        public ClientsController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /*
         *  管理端获取客户列表
         *  /api/administrators/clients
         *  
         *  return:
         *      200:    successfully
         *  
         */
        [HttpGet]
        public async Task<IActionResult> GetClientsListAsync(int index, int size, string s)
        {
            var pager = Domain.Paginator.New(index, size, 1);
            pager["s"] = s ?? "";

            pager = await _userManager.GetUserListAsync(pager, Domain.Users.User.RoleCategories.Client);
            return Ok(pager);
        }

        /*
         *  获取客户详情
         *  
         *  /api/administrators/clients/{id}
         *  
         *  return:
         *      200:    successfully
         *      404:    not exist client
         */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientDetailAsync(string id)
        {
            var client = await _userManager.GetClientAsync(id);
            if (client == null)
                return NotFound();
            var detail = await client.GetDetailAsync();
            return Ok(detail);
        }
    }
}
