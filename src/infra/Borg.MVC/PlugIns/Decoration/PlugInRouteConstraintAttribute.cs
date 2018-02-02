using System;

namespace Borg.MVC.PlugIns.Decoration
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class PlugInRouteConstraintAttribute : Attribute
    {
        public string Name { get; }

        public PlugInRouteConstraintAttribute(string name) : base()
        {
            Name = name;
        }
    }
}