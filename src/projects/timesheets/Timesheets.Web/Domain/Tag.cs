using System;
using Timesheets.Web.Domain.Infrastructure;

namespace Timesheets.Web.Domain
{
    public class Tag : Entity<Guid>
    {
        protected Tag()
        {
            
        }
        public Tag(string value)
        {
            SetValue(value);
            Id = Guid.NewGuid();
        }
        public string Value { get; protected set; }
        public bool IsEnabled { get; protected set; } = true;
        public void SetValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));
            if (Value.Equals(value)) return;
            Value = value;
        }
    }
}
