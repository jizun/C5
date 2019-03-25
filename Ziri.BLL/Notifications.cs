using System;
using System.Linq;
using System.Collections.Generic;
using Ziri.MDL;
using Ziri.DAL;

namespace Ziri.BLL
{
    public class Notifications
    {
        //获取新通知列表
        public static List<WorkbenchNotifications> GetWorkbenchNotifications(long UserID)
        {
            using (var EF = new EF())
            {
                return (from notifications in EF.Notifications
                        join userNotifications in EF.UserNotifications on notifications.ID equals userNotifications.NotificationsID
                        where notifications.HaveReadTime == null && userNotifications.UserID == UserID
                        orderby notifications.UpdateTime descending
                        select new WorkbenchNotifications
                        {
                            ID = notifications.ID,
                            ModuleTypeID = notifications.ModuleTypeID,
                            ModuleTypeIcon = "la-user-plus",
                            Description = notifications.Description,
                            ProcessURL = notifications.ProcessURL,
                            UpdateTime = notifications.UpdateTime,
                            HaveReadTime = notifications.HaveReadTime
                        }).ToList();
            }
        }

        //标记通知已读
        public static void SetHaveRead(long NotificationsID)
        {
            using (var EF = new EF())
            {
                var notifications = EF.Notifications.Where(i => i.ID == NotificationsID).FirstOrDefault();
                if (notifications != null)
                {
                    notifications.HaveReadTime = DateTime.Now;
                    EF.SaveChanges();
                }
            }
        }
    }
}
