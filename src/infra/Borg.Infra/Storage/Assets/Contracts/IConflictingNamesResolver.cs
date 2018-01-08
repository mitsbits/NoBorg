using System.Threading.Tasks;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public interface IConflictingNamesResolver
    {
        Task<string> Resolve(string filename);
    }
}