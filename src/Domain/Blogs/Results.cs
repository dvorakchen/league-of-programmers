using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Blogs
{
    public class Results
    {
        public class BlogDetail
        {
            public string Title { get; set; } = "";
            public string[] Targets { get; set; } = Array.Empty<string>();
            public string Content { get; set; } = "";
            public int Views { get; set; } = 0;
            public string Author { get; set; } = "";
            public string AuthorAccount { get; set; } = "";
            public string DateTime { get; set; } = "";
        }

        public class BlogItem
        {
            public int Id { get; set; } = 0;
            public string Title { get; set; } = "";
            public string Author { get; set; } = "";
            public string AuthorAccount { get; set; } = "";
            public string DateTime { get; set; } = "";
            public int Views { get; set; } = 0;
            public KeyValuePair<int, string> State { get; set; }
        }
    }
}
