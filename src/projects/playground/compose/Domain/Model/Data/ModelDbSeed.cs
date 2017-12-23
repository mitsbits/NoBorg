using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain.Model.Data
{
    public class ModelDbSeed
    {
        private readonly ModelDbContext _db;

        public ModelDbSeed(ModelDbContext db)
        {
            _db = db;
        }


        public async Task Init()
        {
            await _db.Database.MigrateAsync();

        }


    }
}
