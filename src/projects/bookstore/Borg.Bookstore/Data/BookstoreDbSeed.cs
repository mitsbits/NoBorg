using Borg.Platform.EF.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Threading.Tasks;

namespace Borg.Bookstore.Data
{
    public class BookstoreDbSeed : IDbSeed
    {
        private readonly BookstoreDbContext _db;
        private readonly ILogger _logger;

        public BookstoreDbSeed(ILoggerFactory loggerFactory, BookstoreDbContext db)
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