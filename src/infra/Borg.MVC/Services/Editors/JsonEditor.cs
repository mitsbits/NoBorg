using Newtonsoft.Json;
using System;

namespace Borg.MVC.Services.Editors
{
    public sealed class JsonEditor : Editor
    {
        public JsonEditor() : base()
        {
        }

        public JsonEditor(string value) : base()
        {
            Value = value;
        }

        [JsonIgnore]
        public string Value
        {
            get => GetValue<string>(nameof(Value));
            set => SetValue(nameof(Value), value);
        }

        public override Tuple<int, string>[] ValueModel()
        {
            return new[] { Tuple.Create(0, Value) };
        }
    }

    public sealed class JsonEditorDescriptor : EditorDescriptor
    {
        public override string FriendlyName => "Json Editor";
        public override string EditorType => typeof(JsonEditor).FullName;
    }
}