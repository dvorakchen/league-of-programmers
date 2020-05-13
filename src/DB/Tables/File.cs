using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DB.Tables
{
    public class File : Entity
    {
        [Required, StringLength(64)]
        public string Name { get; set; } = "";
        [Required, StringLength(8)]
        public string ExtensionName { get; set; } = "";
        [Required, StringLength(64)]
        public string SaveName { get; set; } = "";
        [Required]
        public long Size { get; set; } = 0;
    }
}
