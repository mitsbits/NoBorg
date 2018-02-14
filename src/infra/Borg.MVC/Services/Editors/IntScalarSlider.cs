using Newtonsoft.Json;

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

        public override (int index, string value)[] ValueModel()
        {
            return new[] { (index: 0, value: Value.ToString()) };
        }
    }

    public sealed class IntScalarSliderDescriptor : EditorDescriptor
    {
        public override string FriendlyName => "Valaue Slider";
        public override string EditorType => typeof(IntScalarSlider).FullName;
    }
}