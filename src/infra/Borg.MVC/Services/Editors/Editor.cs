using Borg.Infra.DTO;
using Borg.Infra.Services;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Borg.MVC.Services.Editors
{
    public abstract class Editor : IPropertyBag //: Tidings
    {
        protected Editor()
        {
            HtmlId = Randomize.String(16);
        }

        public List<Tiding> OptionsBag { get; set; } = new List<Tiding>();

        [JsonIgnore]
        public string HtmlId
        {
            get => GetValue<string>(nameof(HtmlId));
            set => SetValue(nameof(HtmlId), value);
        }

        [JsonIgnore]
        public bool HideLabel
        {
            get => GetValue<bool>(nameof(HideLabel));
            set => SetValue(nameof(HideLabel), value);
        }

        protected bool ContainsKey(string key)
        {
            return OptionsBag.Any(x => x.Key == key);
        }

        public abstract (int index, string value)[] ValueModel();

        public void SetValue<TValue>(string propName, TValue propValue)
        {
            if (ContainsKey(propName))
            {
                var hit = OptionsBag.First(x => x.Key == propName);
                OptionsBag.Remove(hit);
            }
            var value = JsonConvert.SerializeObject(propValue, Formatting.Indented, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.None });
            OptionsBag.Add(new Tiding(propName, value));
        }

        public TValue GetValue<TValue>(string propName)
        {
            if (!ContainsKey(propName)) { SetValue(propName, default(TValue)); };

            return JsonConvert.DeserializeObject<TValue>(OptionsBag.First(x => x.Key == propName).Value, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.None });
        }

        public void SetValueRaw(string propName, string propValue)
        {
            if (ContainsKey(propName))
            {
                var hit = OptionsBag.First(x => x.Key == propName);
                OptionsBag.Remove(hit);
            }

            OptionsBag.Add(new Tiding(propName, propValue));
        }

        public string GetValueRaw(string propName)
        {
            return !ContainsKey(propName) ? string.Empty : OptionsBag.First(x => x.Key == propName).Value;
        }
    }
}