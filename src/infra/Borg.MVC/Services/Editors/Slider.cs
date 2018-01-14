using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Borg.MVC.Services.Editors
{
    public abstract class Slider : Editor
    {
        protected Slider() : base()
        {
        }

        protected Slider(int min, int max, int step, Orientation orientation, SliderColor sliderColor)
        {
            Min = min;
            Max = max;
            Step = step;
            Orientation = orientation;
            SliderColor = sliderColor;
        }

        [JsonIgnore]
        public int Step
        {
            get => GetValue<int>(nameof(Step));
            set => SetValue(nameof(Step), value);
        }

        [JsonIgnore]
        public int Max
        {
            get => GetValue<int>(nameof(Max));
            set => SetValue(nameof(Max), value);
        }

        [JsonIgnore]
        public int Min
        {
            get => GetValue<int>(nameof(Min));
            set => SetValue(nameof(Min), value);
        }

        [JsonIgnore]
        public SliderColor SliderColor
        {
            get => GetValue<SliderColor>(nameof(SliderColor));
            set => SetValue(nameof(SliderColor), value);
        }

        [JsonIgnore]
        [JsonConverter(typeof(StringEnumConverter))]
        public Orientation Orientation
        {
            get => GetValue<Orientation>(nameof(Orientation));
            set => SetValue(nameof(Orientation), value);
        }
    }
}