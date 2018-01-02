using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IHaveSections
    {
        ICollection<ISection> Sections { get; }
        string RenderScheme { get; }

        void SectionsClear();

        void SectionAdd(ISection section);
    }
}