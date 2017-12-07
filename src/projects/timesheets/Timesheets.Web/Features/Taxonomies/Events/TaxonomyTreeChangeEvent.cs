using MediatR;
using System;

namespace Timesheets.Web.Features.Taxonomies.Events
{
    public class TaxonomyTreeChangeEvent : INotification
    {
        public TaxonomyTreeChangeEvent()
        {
        }

        public TaxonomyTreeChangeEvent(Guid taxonomyId)
        {
            TaxonomyId = taxonomyId;
        }

        public Guid TaxonomyId { get; } = Guid.Empty;

        public bool IsGlobal => TaxonomyId == Guid.Empty;
    }
}