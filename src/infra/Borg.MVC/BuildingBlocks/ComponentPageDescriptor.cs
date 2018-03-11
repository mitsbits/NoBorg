using System;
using System.Collections.Generic;
using System.Text;
using Borg.MVC.BuildingBlocks.Contracts;

namespace Borg.MVC.BuildingBlocks
{
    public class ComponentPageDescriptor<TKey> where TKey : IEquatable<TKey>
    {
        public TKey ComponentId { get; set; }
        public string Slug { get; set; }
        public IPageContent PageContent { get; set; }
        public IDeviceStructureInfo Device { get; set; }

    }
}
