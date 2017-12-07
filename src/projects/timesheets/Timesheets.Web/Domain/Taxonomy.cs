using System;
using System.Collections.Generic;
using Timesheets.Web.Domain.Infrastructure;

namespace Timesheets.Web.Domain
{
    public class Taxonomy : Entity<Guid>
    {
        protected Taxonomy()
        {
        }

        public Taxonomy(Guid? parentId, string displayName, bool isEnabled = true)
        {
            Id = Guid.NewGuid();
            DisplayName = displayName;
            IsEnabled = isEnabled;
            ParentId = parentId;
        }

        public Guid? ParentId { get; protected set; }
        public string DisplayName { get; protected set; }

        public bool IsRoot => !ParentId.HasValue;

        public bool IsEnabled { get; protected set; } = true;

        public virtual ICollection<TaxonomyTag> Tags { get; protected set; } = new List<TaxonomyTag>();
    }
}