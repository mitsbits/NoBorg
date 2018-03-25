using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Borg.Infra.DTO;
using Borg.Infra.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Borg.MVC.Services.Editors
{
    public class EnumDropDown : Editor
    {
        public EnumDropDown(Type enumType, object value = null)
        {
            if (!enumType.IsEnum) throw new ArgumentException(nameof(enumType));
            var array = EnumUtil.GetValues(enumType);
            var optionsContainer = new Tiding(nameof(Options));
            foreach (var o in array)
            {
                optionsContainer.Children.Add(((int)o).ToString(), o.ToString());

            }
            OptionsBag.Add(optionsContainer);
            OptionsBag.Add(new Tiding("Value", value?.ToString()));

        }

        [JsonIgnore]
        public string Value
        {
            get => GetValueRaw(nameof(Value));
            set => SetValueRaw(nameof(Value), value);
        }



        [JsonIgnore]
        public IEnumerable<Tiding> Options
        {
            get
            {
                var values = OptionsBag.First(x => x.Key == nameof(Options));
                return values.Children;
            }
        }

        public override (int index, string value)[] ValueModel()=> new[] { (index: 0, value: Value) };


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
}
}
