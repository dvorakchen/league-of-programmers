using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// 各种验证
    /// </summary>
    internal class Validation
    {
        /// <summary>
        /// 验证是否邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool ValidateEmail(string email) => new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,9})\\s*$").IsMatch(email);
        /// <summary>
        /// 验证账号是否只有数字字母和下划线组成
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool ValidateAccount(string account)
        {
            string emailReg = "^[A-Za-z_0-9]{" + Users.User.NAME_MIN_LENGTH + "," + Users.User.NAME_MAX_LENGTH + "}$";
            return new Regex(emailReg).IsMatch(account);
        }
        /// <summary>
        /// 验证用户名，\^%&'.,;=?$ 字符
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ValidateUserName(string userName)
        {
            return (userName.Length <= Users.User.NAME_MAX_LENGTH && userName.Length >= Users.User.NAME_MIN_LENGTH 
                || !new Regex("[" + Users.User.NAME_NOT_ALLOW_CHAR + "]+").IsMatch(userName)) ;
        }
    }
}
