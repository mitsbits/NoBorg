using Borg.Infra.ExtensionMethods;
using Borg.MVC;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Borg.Cms.Basic.Lib.System.Data;

namespace Borg.Cms.Basic.Lib.Features.Auth.Data
{
    public class BorgDbSeed
    {
        private readonly BorgDbContext _db;
        private readonly BorgSettings _settings;
 
        private readonly ILogger _logger;

        public BorgDbSeed(ILoggerFactory loggerFactory, BorgDbContext db, BorgSettings settings)
        {
            _logger = loggerFactory.CreateLogger(GetType());
            _db = db;
            _settings = settings;

        }

        public async Task EnsureUp()
        {
            await _db.Database.MigrateAsync();
  
        }

    }
}