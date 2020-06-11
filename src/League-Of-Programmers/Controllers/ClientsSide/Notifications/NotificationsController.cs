using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Notifications;
using League_Of_Programmers.Controllers.Clients;
using Microsoft.AspNetCore.Mvc;


namespace League_Of_Programmers.Controllers.ClientsSide.Notifications
{
    public class NotificationsController : ClientsSideController
    {
        /*
         *  获取通知列表
         *  
         *  /api/clients/notifications
         *  
         *  return:
         *      200:    successfully
         */
        [HttpGet]
        public async Task<IActionResult> GetNotificationListAsync(int index, int size, string s)
        {
            var pager = Domain.Paginator.New(index, size, 1);
            pager["s"] = s ?? "";

            NotificationsManager manager = new NotificationsManager();
            pager = await manager.GetListAsync(pager);
            return Ok(pager);
        }
    }
}
