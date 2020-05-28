using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    public class File : Entity
    {
        [Required, StringLength(64)]
        public string Name { get; set; } = "";
        [Required, StringLength(8)]
        public string Extension { get; set; } = "";
        [Required, StringLength(64)]
        public string SaveName { get; set; } = "";
        [Required]
        public long Size { get; set; } = 0;
        /// <summary>
        /// 图片缩略图或封面，如果小于64字节就不需要压缩了
        /// </summary>
        [Required, StringLength(64)]
        public string Thumbnail { get; set; }
    }
}
