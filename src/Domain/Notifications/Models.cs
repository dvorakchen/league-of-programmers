namespace Domain.Notifications
{
    public class Models
    {
        public class NewNotification
        {
            public string Title { get; set; }
            public string Content { get; set; }
            public bool IsTop { get; set; } = false;
        }
    }
}
