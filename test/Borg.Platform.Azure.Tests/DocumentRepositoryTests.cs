using Borg.Platform.Azure.Cosmos;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Borg.Platform.Azure.Tests
{
    public class DocumentRepositoryTests
    {
        private readonly DocumentClient _client;

        public DocumentRepositoryTests()
        {
            _client = new DocumentClient(new Uri(Emulator.CosmosConnection.Endpoint), Emulator.CosmosConnection.AuthKey, new ConnectionPolicy { EnableEndpointDiscovery = false });
        }

        [Fact]
        public async Task test_documentdb_repository()
        {
            IDocumentDbRepository<Poco> repo = new DocumentDbRepository<Poco>(new NullLoggerFactory(), Emulator.CosmosConnection);
            var doc1 = await repo.Create(new Poco() { Bar = "two", Foo = "one" });
            var doc2 = await repo.Create(new Poco() { Bar = "four", Foo = "three" });
            var doc3 = await repo.Create(new Poco() { Bar = "six", Foo = "five" });
            doc1.ShouldNotBeNull();
            doc2.ShouldNotBeNull();
            doc3.ShouldNotBeNull();
            var id1 = doc1.Id;
            var id2 = doc2.Id;
            var id3 = doc3.Id;
            var list = await repo.Find(x => true);
            list.ShouldNotBeNull();
            list.Count().ShouldBe(3);
            list = await repo.Find(x => x.Bar == "two");
            list.Count().ShouldBe(1);
            await repo.Delete(x => x.Bar == "two");
            list = await repo.Find(x => true);
            list.Count().ShouldBe(2);
            await repo.Delete(id2);
            await repo.Delete(id3);
            list = await repo.Find(x => true);
            list.Count().ShouldBe(0);
            await DeleteDatabase();
        }

        private async Task DeleteCollection()
        {
            await _client.DeleteDocumentCollectionAsync(
                UriFactory.CreateDocumentCollectionUri(Emulator.CosmosConnection.Collection, Emulator.CosmosConnection.Collection));
        }

        private async Task DeleteDatabase()
        {
            try
            {
                await _client.DeleteDatabaseAsync(UriFactory.CreateDatabaseUri(Emulator.CosmosConnection.Database));
            }
            catch { }
        }
    }

    internal class Poco
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "foo")]
        public string Foo { get; set; }

        [JsonProperty(PropertyName = "bar")]
        public string Bar { get; set; }
    }
}