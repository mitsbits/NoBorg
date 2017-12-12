using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class ComposeDbContext : IdentityDbContext<ComposeUser>
    {
        public ComposeDbContext(DbContextOptions<ComposeDbContext> options)
            : base(options)
        {
        }
    }

    public class ComposeUser : IdentityUser
    {
    }
}