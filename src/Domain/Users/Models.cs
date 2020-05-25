using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users
{
    /// <summary>
    /// required models
    /// </summary>
    public class Models
    {
        public class Login
        {
            public string Account { get; set; } = "";
            public string Password { get; set; } = "";
        }

        public class Register
        {
            public string Account { get; set; } = "";
            public string Password { get; set; } = "";
            public string ConfirmPassword { get; set; } = "";
        }
    }
}
