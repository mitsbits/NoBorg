using Newtonsoft.Json;

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

        public override (int index, string value)[] ValueModel()
        {
            return new[]
            {
                (index: 0, value: MinValue.ToString()),
                (index: 1, value: MaxValue.ToString()),
            };
        }
    }

    public sealed class IntRangeSliderDescriptor : EditorDescriptor
    {
        public override string FriendlyName => "Range Slider";
        public override string EditorType => typeof(IntRangeSlider).FullName;
    }
}