using Borg.Infra.DDD.Contracts;
using Borg.Platform.EF.Instructions.Attributes;
using Microsoft.AspNetCore.Identity;

namespace Borg.Platform.Identity.Data.Entities
{

    public class GenericUser : IdentityUser, IEntity<string>
    {
    }
}