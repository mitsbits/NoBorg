﻿using Newtonsoft.Json;
using System;
using System.Linq;

namespace Borg.MVC.Services.Editors
{
    public sealed class AutoSuggestRangeTextBox : Editor
    {
        public AutoSuggestRangeTextBox() : base()
        {
        }

        public AutoSuggestRangeTextBox(string value = "", params string[] options) : this()
        {
            Value = value;
            Options = options == null || !options.Any() ? new string[0] : options;
        }

        [JsonIgnore]
        public string Value
        {
            get => GetValue<string>(nameof(Value));
            set => SetValue(nameof(Value), value);
        }

        [JsonIgnore]
        public string[] Options
        {
            get => GetValue<string[]>(nameof(Options));
            set => SetValue(nameof(Options), value);
        }

        public override Tuple<int, string>[] ValueModel()
        {
            return new[] { Tuple.Create(0, Value) };
        }
    }

    public sealed class AutoSuggestRangeTextBoxDescriptor : EditorDescriptor
    {
        public override string FriendlyName => "Auto Sugges Text Box";
        public override string EditorType => typeof(AutoSuggestRangeTextBox).FullName;
    }
}