using Borg.Infra.DTO;
using Borg.MVC.BuildingBlocks.Contracts;
using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks
{
    public abstract class BaseModule : IModule<Tidings>
    {
        protected BaseModule(string friendlyName)
            : this()
        {
            FriendlyName = friendlyName;
        }

        protected BaseModule(string friendlyName, IDictionary<string, string> parameters)
            : this(friendlyName)
        {
            Parameters.AppendAndUpdate(parameters);
        }

        protected BaseModule()
        {
            Parameters = new Tidings();
        }

        public abstract ModuleGender ModuleGender { get; }

        public string FriendlyName { get; private set; }

        public Tidings Parameters { get; private set; }

        protected string GetInternalValue(string keyPrefix, string key)
        {
            var dickey = string.Format("{0}{1}", keyPrefix, key);
            return Parameters.ContainsKey(dickey) ? Parameters[dickey] : string.Empty as string;
        }

        protected void SetInternalValue(string keyPrefix, string key, string value)
        {
            var dickey = string.Format("{0}{1}", keyPrefix, key);
            if (!Parameters.ContainsKey(dickey))
            {
                Parameters.Add(dickey, value);
            }
            else
            {
                Parameters[dickey] = value;
            }
        }
    }
}