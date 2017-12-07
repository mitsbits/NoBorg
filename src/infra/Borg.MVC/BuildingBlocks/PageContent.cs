﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.MVC.BuildingBlocks
{
    public class PageContent : IPageContent
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }

        public string[] Body { get; set; }

        public void SetTitle(string title)
        {
            Title = title.Trim();
        }
    }
}
