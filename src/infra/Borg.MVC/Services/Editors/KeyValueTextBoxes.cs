using Borg.Infra.DTO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Borg.MVC.Services.Editors
{
    public sealed class KeyValueTextBoxes : Editor
    {
        public KeyValueTextBoxes() : base()
        {
        }

        public KeyValueTextBoxes(IDictionary<string, string> values, string[] options) : base()
        {
            Options = options == null || !options.Any() ? new List<Tiding>() : options.Select(x => new Tiding() { Key = x, Value = x, Hint = "option" }).ToList();
            ValueContainers = Options.Select(x => new Tiding(x.Key,
                values.ContainsKey(x.Key) ? values[x.Key] : x.Value)
            { Flag = (values.ContainsKey(x.Key)).ToString() }).ToArray();
            KeyHeader = "Key";
            ValueHeader = "Value";
        }

        public KeyValueTextBoxes(IDictionary<string, string> values, IDictionary<string, string> options) : this()
        {
            Options = options == null || !options.Any() ? new List<Tiding>() : options.Select(x => new Tiding() { Key = x.Key, Value = x.Value, Hint = "option" }).ToList();
            ValueContainers = Options.Select(x => new Tiding(x.Key, values.ContainsKey(x.Key) ? values[x.Key] : x.Value) { Flag = (values.ContainsKey(x.Key)).ToString() }).ToArray();
            KeyHeader = "Key";
            ValueHeader = "Value";
        }

        [JsonIgnore]
        public string ValueHeader
        {
            get => GetValue<string>(nameof(ValueHeader));
            set => SetValue(nameof(ValueHeader), value);
        }

        [JsonIgnore]
        public string KeyHeader
        {
            get => GetValue<string>(nameof(KeyHeader));
            set => SetValue(nameof(KeyHeader), value);
        }

        [JsonIgnore]
        public Tiding[] ValueContainers
        {
            get => GetValue<Tiding[]>(nameof(ValueContainers));
            set => SetValue(nameof(ValueContainers), value);
        }

        [JsonIgnore]
        public List<Tiding> Options
        {
            get => GetValue<List<Tiding>>(nameof(Options));
            set => SetValue(nameof(Options), value);
        }

        public override (int index, string value)[] ValueModel()
        {
            return ValueContainers.Select((x, i) => (index: i, value: x.Value)).ToArray();
        }
    }

    public sealed class KeyValueTextBoxesDescriptor : EditorDescriptor
    {
        public override string FriendlyName => "Key Value Text Boxes";
        public override string EditorType => typeof(KeyValueTextBoxes).FullName;
    }
}