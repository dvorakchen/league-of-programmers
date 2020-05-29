using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users
{
    /// <summary>
    /// 客户
    /// </summary>
    public class Client : User
    {
        internal Client(DB.Tables.User userModel): base(userModel)
        {
        }

        /// <summary>
        /// Client: <管理员名>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Client)}: " + base.ToString();
        }
    }
}
