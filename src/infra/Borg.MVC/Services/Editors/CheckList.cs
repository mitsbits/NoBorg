using Borg.Infra.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Borg.MVC.Services.Editors
{
    public sealed class CheckList : Editor
    {
        public CheckList() : base()
        {
        }

        public CheckList(string[] values, Orientation orientation, string[] options) : base()
        {
            Orientation = orientation;
            Options = options == null || !options.Any() ? new List<Tiding>() : options.Select(x => new Tiding() { Key = x, Value = x, Hint = "option" }).ToList();
            ValueContainers = Options.Select(x => new Tiding(x.Key, x.Value) { Flag = values.Contains(x.Key).ToString() }).ToArray();
        }

        public CheckList(string[] values, Orientation orientation, IDictionary<string, string> options) : this()
        {
            Orientation = orientation;
            Options = options == null || !options.Any() ? new List<Tiding>() : options.Select(x => new Tiding() { Key = x.Key, Value = x.Value, Hint = "option" }).ToList();
            ValueContainers = Options.Select(x => new Tiding(x.Key, x.Value) { Flag = values.Contains(x.Key).ToString() }).ToArray();
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

        [JsonIgnore]
        public Orientation Orientation
        {
            get => GetValue<Orientation>(nameof(Orientation));
            set => SetValue(nameof(Orientation), value);
        }

        public override Tuple<int, string>[] ValueModel()
        {
            return ValueContainers.Select((x, i) => Tuple.Create(i, x.Value)).ToArray();
        }
    }

    public sealed class CheckListDescriptor : EditorDescriptor
    {
        public override string FriendlyName => "Check List";
        public override string EditorType => typeof(CheckList).FullName;
    }
}