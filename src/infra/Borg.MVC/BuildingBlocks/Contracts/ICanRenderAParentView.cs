﻿using Borg.Infra.DTO;
using Borg.MVC.Services.Breadcrumbs;

namespace Borg.MVC.BuildingBlocks.Contracts
{
    public interface ICanRenderAParentView
    {
        Tidings Scripts { get; }
        Breadcrumbs Breadcrumbs { get; }
    }
}