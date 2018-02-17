using Microsoft.Extensions.Logging;
using System;

namespace Borg.Infra.Services
{
    public static class ApplicationLogging
    {
        private static ILoggerFactory _loggerFactory;

        public static void SetFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public static ILogger CreateLogger(Type type) => _loggerFactory.CreateLogger(type);

        public static ILogger CreateLogger<TType>() => _loggerFactory.CreateLogger<TType>();
    }
}