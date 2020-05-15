using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DB.Tables
{
    public class User : Entity
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        [Required, StringLength(18)]
        public string Account { get; set; } = "";
        /// <summary>
        /// 用户名字
        /// </summary>
        [Required, StringLength(18)]
        public string Name { get; set; } = "";

        [Required, StringLength(64)]
        public string Password { get; set; } = "";
        [Required, StringLength(64)]
        public string Email { get; set; } = "";
        /// <summary>
        /// 头像以二进制数据存储，最大 64 字节
        /// </summary>
        [MaxLength(64)]
        public byte[] Avatar { get; set; }
        
        [Required, StringLength(512)]
        public string Introduction { get; set; } = "";
    }
}
