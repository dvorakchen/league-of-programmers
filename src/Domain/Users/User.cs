using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users
{
    public class User : EntityBase
    {
        private User(DB.Tables.User userModel)
        {
            if (userModel is null)
                throw new ArgumentNullException("user model not found");
            Id = userModel.Id;
            _name = userModel.Name;

            UserCache.SetUserModel(userModel);
        }

        private readonly string _name = "";

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
