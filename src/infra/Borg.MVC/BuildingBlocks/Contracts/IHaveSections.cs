using Borg.CMS.BuildingBlocks.Contracts;
using System.Collections.Generic;

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