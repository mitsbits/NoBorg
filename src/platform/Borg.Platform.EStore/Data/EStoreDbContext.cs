using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EStore.Data
{
    public class EStoreDbContext : BorgDbContext
    {
        public EStoreDbContext(DbContextOptions<EStoreDbContext> options) : base(options)
        {
        }
    }
}