﻿namespace Borg.CMS.Components.Contracts
{
    public interface IHtmlMeta
    {
        string Content { get; }
        string HttpEquiv { get; }
        string Name { get; }
        string Scheme { get; }
        bool ShouldRender { get; }
    }
}