using System;

namespace Timesheets.Web.Domain
{
    public class TaxonomyTag
    {
        public Guid TaxonomyId { get; set; }
        public Guid TagId { get; set; }
        public virtual Taxonomy Taxonomy { get; set; }
        public virtual Tag Tag { get; set; }

    }
}