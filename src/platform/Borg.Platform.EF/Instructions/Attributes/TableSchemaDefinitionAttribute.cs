using System;
using System.Collections.Generic;
using System.Text;

namespace Borg.Platform.EF.Instructions.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public  class TableSchemaDefinitionAttribute : Attribute
    {
        public TableSchemaDefinitionAttribute(string schema)
        {
            Schema = schema;
        }

        public string Schema { get; }
    }
}
