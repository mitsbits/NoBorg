using System;

namespace Timesheets.Web.Domain.Infrastructure
{
 public   interface IEntity<out T> where T: IEquatable<T>
    {
        T Id { get; }
        bool IsTransient();
    }
}
