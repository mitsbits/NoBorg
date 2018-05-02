using System.Threading.Tasks;
using Borg.Platform.EF.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Borg.Platform.EStore.Data
{
    public class EStoreDbSeed : IDbSeed {
        private readonly EStoreDbContext _db;
        private readonly ILogger _logger;

        public EStoreDbSeed(ILoggerFactory loggerFactory, EStoreDbContext db)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
            _db = db;
        }

        public async Task EnsureUp()
        {
            await _db.Database.MigrateAsync();
        }
    }
}