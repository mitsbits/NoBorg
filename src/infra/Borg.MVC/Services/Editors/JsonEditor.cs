using Newtonsoft.Json;

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
            get => GetValueRaw(nameof(Value));
            set => SetValueRaw(nameof(Value), value);
        }

        [JsonIgnore]
        public dynamic Template
        {
            get => GetValue<dynamic>(nameof(Template));
            set => SetValue(nameof(Template), value);
        }

        public override (int index, string value)[] ValueModel()
        {
            return new[] { (index: 0, value: Value) };
        }
    }

    public sealed class JsonEditorDescriptor : EditorDescriptor
    {
        public override string FriendlyName => "Json Editor";
        public override string EditorType => typeof(JsonEditor).FullName;
    }
}