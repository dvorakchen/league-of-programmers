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
            pager.TotalSize = await db.Notifications.CountAsync(whereStatement);
            pager.List = await db.Notifications.AsNoTracking()
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

        /// <summary>
        /// 获取通知详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Results.NotificationDetail> GetNotificitionDetailAsync(int id)
        {
            await using var db = new LOPDbContext();
            var notification = await db.Notifications.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            if (notification is null)
                return null;
            return new Results.NotificationDetail
            { 
                Title = notification.Title,
                Content = notification.Content
            };
        }

        /// <summary>
        /// 删除一个通知
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteNotificationAsync(int id)
        {
            await using var db = new LOPDbContext();
            var notification = await db.Notifications.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            if (notification is null)
                return;
            db.Notifications.Remove(notification);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount != 1)
                throw new Exception("删除一个通知失败");
        }

        /// <summary>
        /// 发布新通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool, string)> PostNowNotificationAsync(Models.NewNotification model)
        {
            if (string.IsNullOrWhiteSpace(model.Title))
                return (false, "标题不能位空");
            if (string.IsNullOrWhiteSpace(model.Content))
                return (false, "内容不能位空");
            DB.Tables.Notification notification = new DB.Tables.Notification
            { 
                Title = model.Title,
                Content = model.Content,
                IsTop = model.IsTop
            };
            await using var db = new LOPDbContext();
            db.Notifications.Add(notification);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return (true, "");
            throw new Exception("发布通知失败");
        }
    }
}
