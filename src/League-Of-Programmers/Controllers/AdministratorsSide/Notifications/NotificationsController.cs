using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Notifications;
using League_Of_Programmers.Controllers.Clients;
using Microsoft.AspNetCore.Mvc;


namespace League_Of_Programmers.Controllers.AdministratorsSide.Notifications
{
    public class NotificationsController : AdministratorsSideController
    {
        /*
         *  删除一个通知
         *  
         *  /api/administrators/notifications
         *  
         *  return:
         *      200:    successfully
         *      400:    fault
         */
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotificationAsync(int id)
        {
            NotificationsManager manager = new NotificationsManager();
            await manager.DeleteNotificationAsync(id);
            return Ok();
        }

        /*
         *  新发布一个通知
         * 
         *  /api/administrators/notifications
         *  
         *  return:
         *      200:    successfully
         *      400:    fault
         */
        [HttpPost]
        public async Task<IActionResult> PostNewNotificationAsync(Models.NewNotification model)
        {
            NotificationsManager manager = new NotificationsManager();
            (bool isSuccessfully, string msg) = await manager.PostNowNotificationAsync(model);
            if (isSuccessfully)
                return Ok();
            return BadRequest(msg);
        }
    }
}
