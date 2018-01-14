using Newtonsoft.Json;
using System;

namespace Borg.MVC.Services.Editors
{
    public sealed class IntRangeSlider : Slider
    {
        public IntRangeSlider() : base()
        {
        }

        public IntRangeSlider(int minValue = -1, int maxValue = 1, int min = -100, int max = 100, int step = 1,
            Orientation orientation = Orientation.Horizontal, SliderColor color = SliderColor.Empty) : base(min, max, step, orientation, color)
        {
            MinValue = minValue;
            MaxValue = maxValue;
        }

        [JsonIgnore]
        public int MinValue
        {
            get => GetValue<int>(nameof(MinValue));
            set => SetValue(nameof(MinValue), value);
        }

        [JsonIgnore]
        public int MaxValue
        {
            get => GetValue<int>(nameof(MaxValue));
            set => SetValue(nameof(MaxValue), value);
        }

        public override Tuple<int, string>[] ValueModel()
        {
            var mn = Tuple.Create(0, MinValue.ToString());
            var mx = Tuple.Create(1, MaxValue.ToString());
            return new[] { mn, mx };
        }
    }

    public sealed class IntRangeSliderDescriptor : EditorDescriptor
    {
        public override string FriendlyName => "Range Slider";
        public override string EditorType => typeof(IntRangeSlider).FullName;
    }
}