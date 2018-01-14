using Newtonsoft.Json;
using System;

namespace Borg.MVC.Services.Editors
{
    public sealed class IntScalarSlider : Slider
    {
        public IntScalarSlider() : base()
        {
        }

        public IntScalarSlider(int value = 0, int min = -100, int max = 100, int step = 1,
            Orientation orientation = Orientation.Horizontal, SliderColor color = SliderColor.Empty) : base(min, max, step, orientation, color)
        {
            Value = value;
        }

        [JsonIgnore]
        public int Value
        {
            get => GetValue<int>(nameof(Value));
            set => SetValue(nameof(Value), value);
        }

        public override Tuple<int, string>[] ValueModel()
        {
            return new[] { Tuple.Create(0, Value.ToString()) };
        }
    }

    public sealed class IntScalarSliderDescriptor : EditorDescriptor
    {
        public override string FriendlyName => "Valaue Slider";
        public override string EditorType => typeof(IntScalarSlider).FullName;
    }
}