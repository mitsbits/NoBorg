using System;
using System.Collections.Generic;
using System.Linq;
using Borg.Infra.DTO;

namespace Borg.Infra.Services
{
    public static class EnumUtil
    {
        public static Array GetValues(Type enumtype)
        {
            if (!enumtype.IsEnum) throw new ArgumentException(nameof(enumtype));
            return Enum.GetValues(enumtype);


        }
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }
}