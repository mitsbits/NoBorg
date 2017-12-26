using Microsoft.EntityFrameworkCore;

namespace Borg.Platform.EF.Contracts
{
    public interface IHaveDbContext<out TDbContext> where TDbContext : DbContext
    {
        TDbContext Context { get; }
    }
}