using System.ComponentModel.DataAnnotations;

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
        /// 头像
        /// </summary>
        public int AvatarId { get; set; } = 1;
        public File Avatar { get; set; }
        [Required, StringLength(512)]
        public string Introduction { get; set; } = "";
    }
}
