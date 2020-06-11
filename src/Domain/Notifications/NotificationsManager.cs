using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Notifications
{
    public class NotificationsManager
    {

        /// <summary>
        /// 获取通知列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public async Task<Paginator> GetListAsync(Paginator pager)
        {
            string s = pager["s"] ?? "";

            Expression<Func<DB.Tables.Notification, bool>> whereStatement = n => true;
            if (!string.IsNullOrWhiteSpace(s))
                whereStatement = whereStatement.And(n => n.Title.Contains(s));

            await using var db = new LOPDbContext();
            pager.TotalSize = await db.Notificatios.CountAsync(whereStatement);
            pager.List = await db.Notificatios.AsNoTracking()
                                              .OrderByDescending(n => n.IsTop)
                                              .ThenByDescending(n => n.CreateDate)
                                              .Where(whereStatement)
                                              .Skip(pager.Skip)
                                              .Take(pager.Size)
                                              .Select(n => new Results.NotificationItem 
                                              {
                                                  Id = n.Id,
                                                  Title = n.Title,
                                                  DateTime = n.CreateDate.ToString("yyyy/MM/dd HH:mm"),
                                                  IsTop = n.IsTop
                                              })
                                              .ToListAsync();
            return pager;
        }
    }
}
