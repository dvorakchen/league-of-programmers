using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Blogs
{
    public class Models
    {
        public class NewPost
        {
            public string Title { get; set; } = "";
            public string[] Targets { get; set; }
            public string Content { get; set; } = "";
            public int AuthorId { get; set; } = 0;
            public bool IsDraft { get; set; } = false;
        }

        public class ModifyPost
        {
            public string Title { get; set; } = "";
            public string[] Targets { get; set; }
            public string Content { get; set; } = "";
        }
    }
}
