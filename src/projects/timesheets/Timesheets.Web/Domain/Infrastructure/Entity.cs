using System;


namespace Timesheets.Web.Domain.Infrastructure
{
    public class Entity<T> : IEntity<T> where T : IEquatable<T>
    {
        public T Id { get; protected set; }

        public bool IsTransient()
        {
            return Id.Equals(default(T));
        }
    }
}
