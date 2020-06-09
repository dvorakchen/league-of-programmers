using System;

namespace Domain
{
    public abstract class EntityBase
    {
        public int Id { get; protected set; }

        protected void DiagnosisNull(object obj)
        {
            if (obj is null)
                throw new NullReferenceException();
        }
    }
}
