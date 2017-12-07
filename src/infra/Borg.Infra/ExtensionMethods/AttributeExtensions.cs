using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.Infra.ExtensionMethods
{
    public static class AttributeExtensions
    {
        public static T GetCustomAttribute<T>(this Type type) where T : Attribute
        {
            var attrs = type.GetCustomAttributes(typeof(T), true);
            return attrs.Any() ? (T)attrs.First() : null;
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this Type type) where T : Attribute
        {
            var attrs = type.GetCustomAttributes(typeof(T), true);
            return attrs.Any() ? attrs.Cast<T>() : null;
        }
    }
}