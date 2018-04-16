using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Borg.CMS.BackOfficeInstructions
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EditorTabAttribute : Attribute
    {
        public EditorTabAttribute(string tab)
        {
            Tab = tab;
        }

        public string Tab { get; }
    }

    public static class EditorAttributeHelper
    {
        public static string[] PropertyTabs(Type type)
        {
            var props = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(EditorTabAttribute)));
            return props.Select(x => x.GetCustomAttribute<EditorTabAttribute>().Tab).Distinct().OrderBy(x => x).ToArray();
        }

        public static IDictionary<string, PropertyInfo[]> ProperiesByTabs(Type type)
        {
            var result = type.GetProperties().Where(x => x.GetMethod.IsPublic).GroupBy(x =>
             {
                 var attr = x.GetCustomAttribute<EditorTabAttribute>();
                 return attr == null ? type.Name : attr.Tab;
             }).ToDictionary(x => x.Key, x => x.ToArray());
            return result;
        }
    }
}