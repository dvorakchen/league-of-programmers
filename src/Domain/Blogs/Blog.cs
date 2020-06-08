using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Blogs
{
    public class Blog : EntityBase
    {
        internal Blog(DB.Tables.Blog model)
        {
            if (model is null)
                throw new NullReferenceException();
            Id = model.Id;
        }

    }
}
