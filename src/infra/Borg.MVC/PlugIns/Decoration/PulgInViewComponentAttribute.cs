using System;

namespace Borg.MVC.PlugIns.Decoration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PulgInViewComponentAttribute : Attribute
    {
        public PulgInViewComponentAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}