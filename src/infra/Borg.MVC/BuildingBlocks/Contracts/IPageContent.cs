using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.MVC.BuildingBlocks
{
    public interface IPageContent
    {
        string Title { get; }
        string Subtitle { get; }
        string[] Body { get; }

        void SetTitle(string title);
    }
}
