using Borg.MVC.BuildingBlocks.Contracts;
using System;

namespace Borg.MVC.BuildingBlocks
{
    public class SectionSlotInfo : ISectionSlotInfo
    {
        protected SectionSlotInfo()
        {
        }

        public SectionSlotInfo(string sectionIdentifier, bool enabled, int ordinal)
            : this()
        {
            if (string.IsNullOrWhiteSpace(sectionIdentifier)) throw new ArgumentNullException(nameof(sectionIdentifier));
            SectionIdentifier = sectionIdentifier;
            Enabled = enabled;
            Ordinal = ordinal;
        }

        public string SectionIdentifier { get; private set; }

        public bool Enabled { get; private set; }

        public int Ordinal { get; private set; }

        public SectionSlotInfo SetEnabled(bool state)
        {
            return state == Enabled ? this : new SectionSlotInfo(SectionIdentifier, state, Ordinal);
        }

        public SectionSlotInfo SetIndex(int index)
        {
            return index == Ordinal ? this : new SectionSlotInfo(SectionIdentifier, Enabled, index);
        }
    }
}