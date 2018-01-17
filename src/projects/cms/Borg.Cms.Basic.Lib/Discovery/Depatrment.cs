using Borg.Infra.DDD;
using Borg.Platform.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Borg.Cms.Basic.Lib.System.Data;


namespace Borg.Cms.Basic.Lib.Discovery
{
    [Entity]
    public class Depatrment : IEntity<int>
    {
        [Required]
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Person
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }

    [Entity]
    public class Employee : Person, IEntity<int>
    {
        [Required]
        public int Id { get; set; }

        [LookUp(typeof(Depatrment))]
        public int DepartmentId { get; set; }

        public virtual Depatrment Depatrment { get; set; }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class LookUpAttribute : Attribute
    {
        public LookUpAttribute(Type lookUpType)
        {
            LookUpType = lookUpType;
        }

        public Type LookUpType { get; }
    }

    public class DiscoveryDbContext : BorgDbContext
    {
        public DiscoveryDbContext(DbContextOptions<DiscoveryDbContext> options) : base(options)
        {
        }

        internal IEnumerable<Type> EntityTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }

    public class DiscoveryDbContextFactory : BorgDbContextFactory<DiscoveryDbContext>
    {
    }

    internal static class Util
    {
        public static IEnumerable<Type> FindEntities(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.GetCustomAttribute<EntityAttribute>() != null);
        }
    }
}