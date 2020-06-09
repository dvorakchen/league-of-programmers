using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Blogs.List
{
    internal interface IBlogList
    {
        Task<Paginator> GetListAsync(Paginator pager);
    }
}
