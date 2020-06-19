namespace Domain.Users
{
    public class Results
    {
        public class LoginResult
        {
            public string Account { get; set; }
            public string UserName { get; set; }
            public int Role { get; set; }
        }

        public class ClientHomePageProfile
        {
            public string Avatar { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
        }

        public class UserItem
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string Account { get; set; }
            public string Email { get; set; }
            public int BlogCounts { get; set; }
            public string CreateDate { get; set; }
        }

        public class UserDetail
        {
            public string UserName { get; set; }
            public string Account { get; set; }
            public string Avatar { get; set; }
            public string Email { get; set; }
            public string CreateDate { get; set; }
        }
    }
}
