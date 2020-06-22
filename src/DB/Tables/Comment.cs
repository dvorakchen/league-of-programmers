using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DB.Tables
{
    public class Comment: Entity
    {
        [Required, StringLength(512)]
        public string Content { get; set; } = "";
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
        public int? CommenterId { get; set; }
        public User Commenter { get; set; }
    }
}
