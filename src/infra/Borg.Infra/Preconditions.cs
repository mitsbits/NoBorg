using JetBrains.Annotations;
using System;
using System.Diagnostics;

namespace Borg.Infra
{
    [DebuggerStepThrough]
    internal static class Preconditions
    {
        [ContractAnnotation("value:null => halt")]
        public static T NotNull<T>([NoEnumeration] T value, [InvokerParameterName, NotNull] string parameterName)
            where T : class
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static string NotEmpty(string value, [InvokerParameterName, NotNull] string parameterName)
        {
            if (ReferenceEquals(value, null))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            if (value.Length == 0)
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentException("String value cannot be null.", parameterName);
            }

            return value;
        }

        [ContractAnnotation("value:null => halt")]
        public static DateTime NotEmpty(DateTime value, [InvokerParameterName, NotNull] string parameterName)
        {
            if (value.Equals(default(DateTime)))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        public static TEnum IsDefined<TEnum>(TEnum value, [InvokerParameterName, NotNull] string parameterName) where TEnum : struct
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
            {
                NotEmpty(parameterName, nameof(parameterName));

                throw new ArgumentOutOfRangeException(parameterName);
            }

            return value;
        }
    }
}