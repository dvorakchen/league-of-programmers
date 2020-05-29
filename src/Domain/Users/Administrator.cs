using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Users
{
    /// <summary>
    /// 管理员
    /// </summary>
    public class Administrator : User
    {
        internal Administrator(DB.Tables.User userModel) : base(userModel)
        {
        }

        /// <summary>
        /// Administrator: <管理员名>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Administrator)}: " + base.ToString();
        }
    }
}
