using System.Collections.Generic;
using System.Reflection;

namespace Borg.Infra.Services.AssemblyProvider
{
    public interface IAssemblyProvider
    {
        IEnumerable<Assembly> GetAssemblies();
    }
}