using Microsoft.EntityFrameworkCore;
using System;

namespace Borg.Platform.EF.Exceptions
{
    public class EntityNotMappedException : BorgEfException
    {
        public EntityNotMappedException(Type type) : base(SetExceptionMessage(type))
        {
        }

        private static string SetExceptionMessage(Type type)
        {
            return $"{type.Name} is not mapped to ef entity";
        }
    }

    public class EntityNotMappedException<TDbContext> : BorgEfException where TDbContext : DbContext
    {
        public EntityNotMappedException(Type type) : base(SetExceptionMessage(type))
        {
        }

        private static string SetExceptionMessage(Type type)
        {
            return $"{type.Name} is not mapped to ef entity in db context {typeof(TDbContext).Name}";
        }
    }
}