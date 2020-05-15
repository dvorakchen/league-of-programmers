using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users
{
    public class User : EntityBase
    {
        private User(DB.Tables.User userModel)
        {
            Id = userModel.Id;
            _name = userModel.Name;
        }

        private string _name = "";

        /// <summary>
        /// user name
        /// </summary>
        public string Name => _name;

        public static User Parse(DB.Tables.User userModel)
        {
            return new User(userModel);
        }
    }
}
