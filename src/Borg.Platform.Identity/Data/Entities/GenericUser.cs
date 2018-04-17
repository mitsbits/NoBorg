using Borg.Infra.DDD.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Borg.Platform.Identity.Data.Entities
{
    public class GenericUser : IdentityUser, IEntity<string>
    {
    }
}