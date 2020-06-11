using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DB.Tables
{
    public class Notification : Entity
    {
        [Required, StringLength(64)]
        public string Title { get; set; } = "";
        [Required]
        public string Content { get; set; } = "";
        [Required]
        public bool IsTop { get; set; } = false;
        [Required]
        public int Views { get; set; } = 0;
    }
}
