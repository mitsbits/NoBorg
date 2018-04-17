using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Borg.Infra.Services.Factory
{
    public static class New<T>
    {
        public static readonly Func<T> Instance = Creator();

        private static Func<T> Creator()
        {
            Type type = typeof(T);
            if (type == typeof(string))
                return Expression.Lambda<Func<T>>(Expression.Constant(string.Empty)).Compile();

            if (type.HasDefaultConstructor())
                return Expression.Lambda<Func<T>>(Expression.New(type)).Compile();

            return () => (T)FormatterServices.GetUninitializedObject(type);
        }
    }

    public static class New
    {
        public static object Creator(Type type)
        {
            if (type == typeof(string)) return string.Empty;

            return type.HasDefaultConstructor()
                ? Expression.Lambda<Func<object>>(Expression.New(type)).Compile().Invoke()
                : FormatterServices.GetUninitializedObject(type);
        }
    }
}