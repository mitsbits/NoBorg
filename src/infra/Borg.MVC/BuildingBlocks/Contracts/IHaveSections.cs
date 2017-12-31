using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface IHaveSections
    {
        ICollection<ISection> Sections { get; }
    }
}