using Borg.Infra.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Linq;

namespace Borg.MVC.Services.Editors
{
    public sealed class RadioList : Editor
    {
        public RadioList() : base()
        {
        }

        public RadioList(string value, Orientation orientation, string[] options) : base()
        {
            Value = value;
            Orientation = orientation;
            Options = options == null || !options.Any() ? new List<Tiding>() : options.Select(x => new Tiding() { Key = x, Value = x, Hint = "option" }).ToList();
        }

        public RadioList(string value, Orientation orientation, IDictionary<string, string> options) : this()
        {
            Value = value;
            Orientation = orientation;
            Options = options == null || !options.Any() ? new List<Tiding>() : options.Select(x => new Tiding() { Key = x.Key, Value = x.Value, Hint = "option" }).ToList();
        }

        [JsonIgnore]
        public string Value
        {
            get => GetValue<string>(nameof(Value));
            set => SetValue(nameof(Value), value);
        }

        [JsonIgnore]
        public List<Tiding> Options
        {
            get => GetValue<List<Tiding>>(nameof(Options));
            set => SetValue(nameof(Options), value);
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public Orientation Orientation
        {
            get => GetValue<Orientation>(nameof(Orientation));
            set => SetValue(nameof(Orientation), value);
        }

        public override (int index, string value)[] ValueModel()
        {
            return new[] { (index: 0, value: Value) };
        }
    }

    public sealed class RadioListDescriptor : EditorDescriptor
    {
        public override string FriendlyName => "Radio List";
        public override string EditorType => typeof(RadioList).FullName;
    }
}