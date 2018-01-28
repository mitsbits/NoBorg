using System.Collections.Generic;
using Borg.CMS.BuildingBlocks.Contracts;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IHaveSections
    {
        ICollection<ISection> Sections { get; }
        string RenderScheme { get; set; }

        void SectionsClear();

        void SectionAdd(ISection section);
    }
}