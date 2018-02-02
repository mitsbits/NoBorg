using Borg.Infra;
using System;
using System.Collections.Generic;

namespace Borg.MVC.BuildingBlocks
{
    public class ModuleGender
    {
        private readonly string _flavorDescription = String.Empty;
        private static readonly IDictionary<string, ModuleGender> Dict = new Dictionary<string, ModuleGender>();

        public static readonly ModuleGender Empty = new ModuleGender("Empty");
        public static readonly ModuleGender PartialView = new ModuleGender("View");
        public static readonly ModuleGender ViewComponent = new ModuleGender("Component");

        private ModuleGender(string flavorDescription)
        {
            Preconditions.NotEmpty(flavorDescription, nameof(flavorDescription));
            _flavorDescription = flavorDescription;
            Dict.Add(flavorDescription, this);
        }

        public override string ToString()
        {
            return _flavorDescription;
        }

        public string Flavor { get { return ToString(); } }

        public static ModuleGender Parse(string flavorDescription)
        {
            if (Dict.Keys.Contains(flavorDescription))
            {
                return Dict[flavorDescription];
            }
            throw new NotImplementedException("This type description is not supported currently.");
        }

        public static bool TryParse(string heightDescription, out ModuleGender gender)
        {
            try
            {
                gender = Parse(heightDescription);
                return true;
            }
            catch (NotImplementedException)
            {
                gender = Empty;
                return false;
            }
        }

        public static List<ModuleGender> GetMembers()
        {
            return new List<ModuleGender>(Dict.Values);
        }
    }
}