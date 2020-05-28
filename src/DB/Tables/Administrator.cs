using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DB.Tables
{
    public class Administrator: Entity
    {
        [Required]
        public int Roles { get; set; } = 0;
        [Required, StringLength(64)]
        public string Name { get; set; } = "";
    }
}
