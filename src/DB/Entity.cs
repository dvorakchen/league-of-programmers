using System;
using System.ComponentModel.DataAnnotations;

namespace DB
{
    public abstract class Entity
    {
        [Required, Key]
        public int Id { get; set; }
        [Required]
        public int State { get; set; } = 1;
        [Required]
        public DateTimeOffset CreateDate { get; set; } = DateTimeOffset.Now;
    }
}
