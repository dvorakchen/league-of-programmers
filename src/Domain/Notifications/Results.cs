using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Notifications
{
    public class Results
    {
        public class NotificationItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string DateTime { get; set; }
            public bool IsTop { get; set; }
        }

        public class NotificationDetail
        {
            public string Title { get; set; } = "";
            public string Content { get; set; } = "";
        }
    }
}
