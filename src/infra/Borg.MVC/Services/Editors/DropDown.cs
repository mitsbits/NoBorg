using Borg.Infra.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Borg.MVC.Services.Editors
{
    public sealed class DropDown : Editor
    {
        public DropDown() : base()
        {
        }

        public DropDown(string value = "", params string[] options) : this()
        {
            Value = value;
            Options = options == null || !options.Any() ? new List<Tiding>() : options.Select(x => new Tiding() { Key = x, Value = x, Hint = "option" }).ToList();
        }

        public DropDown(string value, IDictionary<string, string> options) : this()
        {
            Value = value;
            Options = options == null || !options.Any() ? new List<Tiding>() : options.Select(x => new Tiding() { Key = x.Key, Value = x.Value, Hint = "option" }).ToList();
        }

        public DropDown(string value, IDictionary<string, IDictionary<string, string>> options) : this(value)
        {
            if (options != null && options.Any())
            {
                Options = ProduceOptions(options);
            }
            else
            {
                Options = new List<Tiding>();
            }
        }

        [JsonIgnore]
        public string Value
        {
            get => GetValueRaw(nameof(Value));
            set => SetValueRaw(nameof(Value), value);
        }

        [JsonIgnore]
        public List<Tiding> Options
        {
            get => GetValue<List<Tiding>>(nameof(Options));
            set => SetValue(nameof(Options), value);
        }

        public override (int index, string value)[] ValueModel()
        {
            return new[] { (index: 0, value: Value) };
        }

        public IEnumerable<SelectListItem> DropDownItems(bool insertEmpty = false, string emptyvalue = "", string emptydisplay = "...")
        {
            var result = new List<SelectListItem>();
            foreach (var option in Options.AsEnumerable())
            {
                if (option.Hint == "group")
                {
                    var @group = new SelectListGroup() { Name = option.Value };
                    foreach (var item in option.Children)
                    {
                        {
                            if (@group.Name.Length > 0)
                            {
                                result.Add(new SelectListItem() { Value = item.Key, Text = item.Value, Group = @group, Selected = item.Value == Value });
                            }
                            else
                            {
                                result.Add(new SelectListItem() { Value = item.Key, Text = item.Value, Selected = item.Value == Value });
                            }
                        }
                    }
                }
                else
                {
                    result.Add(new SelectListItem() { Value = option.Key, Text = option.Value, Selected = option.Value == Value });
                }
            }
            if (insertEmpty) result.Insert(0, new SelectListItem() { Value = emptyvalue, Text = emptydisplay, Selected = emptyvalue == Value });
            return result;
        }

        private static List<Tiding> ProduceOptions(IDictionary<string, IDictionary<string, string>> options)
        {
            var result = new List<Tiding>();
            foreach (var @group in options.Keys)
            {
                if (string.IsNullOrWhiteSpace(@group)) continue; ;
                var tg = new Tiding(@group, @group) { Hint = "group" };
                foreach (var t in options[@group])
                {
                    tg.Children.Add(new Tiding(t.Key, t.Value) { Hint = "option" });
                }
                result.Add(tg);
            }

            return result;
        }
    }

    public sealed class DropDownDescriptor : EditorDescriptor
    {
        public override string FriendlyName => "Drop Down";
        public override string EditorType => typeof(DropDown).FullName;
    }
}