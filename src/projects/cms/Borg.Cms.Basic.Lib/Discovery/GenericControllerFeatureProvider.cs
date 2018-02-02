using Borg.Cms.Basic.Lib.Discovery.Contracts;
using Borg.MVC.PlugIns.Contracts;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Borg.Cms.Basic.Lib.Discovery
{
    public class EntityControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IPlugInHost _host;

        public EntityControllerFeatureProvider(IPlugInHost host)
        {
            _host = host;
        }

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            var types = _host.SpecifyPlugins<IPlugInEfEntityRegistration>();
            foreach (var entityType in types.SelectMany(x => x.Entities.Keys).Distinct())
            {
                var typeName = entityType.Name + "Controller";
                if (feature.Controllers.All(t => t.Name != typeName) && entityType.GetCustomAttribute<EntityAttribute>() != null)
                {
                    // There's no 'real' controller for this entity, so add the generic version.
                    var controllerType = typeof(EntityController<>)
                        .MakeGenericType(entityType).GetTypeInfo();
                    feature.Controllers.Add(controllerType);
                }
            }
        }
    }
}