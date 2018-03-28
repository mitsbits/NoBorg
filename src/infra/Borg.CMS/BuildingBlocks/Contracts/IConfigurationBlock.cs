using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.CMS.BuildingBlocks.Contracts
{
    public interface IConfigurationBlock
    {
        string Display { get; }
        ISetting Setting { get; }

        string SettingType { get; }
    }

    public interface ISetting { }
}
