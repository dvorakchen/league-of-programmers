using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DB.Tables
{
    public class Target : Entity
    {
        [Required, StringLength(32)]
        public string Name { get; set; } = "";
    }
}
