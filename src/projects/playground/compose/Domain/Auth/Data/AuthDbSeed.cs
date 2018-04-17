using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Domain.Auth.Data
{
    public class AuthDbSeed
    {
        private readonly AuthDbContext _db;

        public AuthDbSeed(AuthDbContext db)
        {
            _db = db;
        }

        public async Task Init()
        {
            await _db.Database.MigrateAsync();
        }
    }
}