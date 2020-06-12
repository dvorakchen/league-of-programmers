using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace League_Of_Programmers.Controllers.Clients
{
    [Authorize/*, ValidateAntiForgeryToken*/]
    [Route("/api/administrators/[controller]")]
    public class AdministratorsSideController : LOPController
    {
        
    }
}
