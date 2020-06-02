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
            public string Account { get; set; }
            public string Email { get; set; }
        }

        public class Blogs
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Abstract { get; set; }
        }
    }
}
