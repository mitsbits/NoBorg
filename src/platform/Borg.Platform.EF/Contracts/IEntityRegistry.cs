using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.Contracts
{
    public interface IEntityRegistry
    {
        void RegisterWithDbContext(ModelBuilder builder);
    }
}