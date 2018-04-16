using System.Threading.Tasks;

namespace Borg.Platform.EF.Contracts
{
    public interface IDbSeed
    {
        Task EnsureUp();
    }
}