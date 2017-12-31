﻿using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface ISection
    {
        string Identifier { get; }

        string FriendlyName { get; }

        ICollection<ISlot> Slots { get; }

        string RenderScheme { get; }
    }
}