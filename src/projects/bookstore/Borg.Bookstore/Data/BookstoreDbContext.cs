using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;

namespace Borg.Bookstore.Data
{
    public class BookstoreDbContext : BorgDbContext
    {
        public BookstoreDbContext(DbContextOptions<BookstoreDbContext> options) : base(options)
        {
        }
    }
}
