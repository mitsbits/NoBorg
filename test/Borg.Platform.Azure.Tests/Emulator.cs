using Borg.Platform.Azure.Cosmos;
using System;

namespace Borg.Platform.Azure.Tests
{
    public class Emulator
    {
        static Emulator()
        {
            CosmosConnection = new DocDbConfig()
            {
                Endpoint = "https://localhost:8081/",
                AuthKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                Database = $"test{DateTimeOffset.UtcNow.Ticks}"
            };
        }

        public const string AzureStorageConnection = "UseDevelopmentStorage=true";

        public static DocDbConfig CosmosConnection { get; }
    }
}