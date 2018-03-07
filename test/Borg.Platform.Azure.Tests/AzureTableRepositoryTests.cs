using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Borg.Platform.Azure.Storage.Tables;
using Borg.Infra.DDD;
using Borg.Infra.DDD.Contracts;

namespace Borg.Platform.Azure.Tests
{
    public class AzureTableRepositoryTests
    {
        private readonly string _tableName = nameof(AzureTableRepositoryTests) + DateTime.Now.Ticks;

        private readonly IAzureTableStoreRepository<ScenarioViewModel> _repo;

        public AzureTableRepositoryTests()
        {
            _repo = new AzureTableJsonRepository<ScenarioViewModel>(Emulator.AzureStorageConnection, _tableName);
        }

        [Fact]
        public void test_that_there_is_a_default_table_name()
        {
            var repo = new AzureTableJsonRepository<ScenarioViewModel>(Emulator.AzureStorageConnection);
            repo._tableName.ShouldBe(typeof(ScenarioViewModel).Name);
            DeleteTable(typeof(ScenarioViewModel).Name).Wait();
        }

        [Fact]
        public async Task test_inserting_and_retrieving_and_deleting_objects()
        {
            var bucket = DecorateWithScreens(MockScenarios());
            var keys = new List<CompositeKey<string>>();

            foreach (var scenario in bucket)
            {
                var inserted = await _repo.Create(scenario);
                inserted.ShouldNotBeNull();
                inserted.CompositeKey.ShouldBe(scenario.CompositeKey);
                keys.Add(inserted.CompositeKey); //create
            }

            foreach (var partitionedKey in keys)
            {
                var hit = await _repo.Get(partitionedKey); //get
                hit.ShouldNotBeNull();
                hit.Screens.Count().ShouldBeGreaterThan(0);
                hit.CompositeKey.ShouldBe(partitionedKey);

                await _repo.Delete(hit.CompositeKey); //delete
            }
            var predicate = string.Empty;
            var scenarios = await _repo.Find(predicate);
            scenarios.Count().ShouldBe(0);
            await DeleteTable();
        }

        #region infra

        private IEnumerable<ScenarioViewModel> DecorateWithScreens(IEnumerable<ScenarioViewModel> source)
        {
            Random r = new Random(3);
            foreach (var scenarioViewModel in source)
            {
                var bucket = new List<ScenarioScreenViewModel>();
                for (var i = 0; i < 12; i++)
                {
                    var screen = new ScenarioScreenViewModel()
                    {
                        ScreenName = ProduceName(r, 10),
                    };
                    screen.ScreenUrl = $"http://futspcdevtab1.westeurope.cloudapp.azure.com/{screen.Id}";
                    bucket.Add(screen);
                }
                scenarioViewModel.Screens = bucket;
                yield return scenarioViewModel;
            }
        }

        private IEnumerable<ScenarioViewModel> MockScenarios()
        {
            var r = new Random();
            for (var i = 0; i < 20; i++)
            {
                yield return new ScenarioViewModel
                {
                    DateCreated = DateTime.Now.AddDays(-r.Next(1, 120)),
                    Description =
                        @"Cards are built with as little markup and styles as possible, but still manage to deliver a ton of control and customization.
                      Built with flexbox, they offer easy alignment and mix well with other Bootstrap components.",
                    ScenarioName = ProduceName(r, r.Next(5, 13)),
                    Status = ProduceState(r)
                };
            }
        }

        private async Task DeleteTable(string tableName = "")
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Emulator.AzureStorageConnection);

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable that represents the "people" table.
            CloudTable table = tableClient.GetTableReference(string.IsNullOrWhiteSpace(tableName) ? _tableName : tableName);

            // Delete the table it if exists.
            await table.DeleteIfExistsAsync();
        }

        private ScenarioStatus ProduceState(Random random)
        {
            return (ScenarioStatus)random.Next(0, 3);
        }

        private string ProduceName(Random random, int limit)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            var builder = new StringBuilder();
            for (var i = 1; i <= limit; i++)
            {
                builder.Append(chars[random.Next(0, chars.Length - 1)]);
            }
            return builder.ToString();
        }

        public class ScenarioViewModel : IHasCompositeKey<string>
        {
            public ScenarioViewModel()
            {
                PartitionKey = GetType().Name;
                Id = Guid.NewGuid();
                RowKey = Id.ToString();
            }

            public string PartitionKey { get; set; }
            public string RowKey { get; set; }
            public Guid Id { get; }
            public string ScenarioName { get; set; }

            [JsonConverter(typeof(StringEnumConverter))]
            public ScenarioStatus Status { get; set; }

            public string Description { get; set; }
            public DateTime DateCreated { get; set; }
            public IEnumerable<ScenarioScreenViewModel> Screens { get; set; }
            public CompositeKey<string> CompositeKey => CompositeKey<string>.Create(PartitionKey, RowKey);
        }

        public class ScenarioScreenViewModel
        {
            public Guid Id { get; } = Guid.NewGuid();
            public string ScreenName { get; set; }
            public string ScreenUrl { get; set; }
        }

        public enum ScenarioStatus
        {
            Processing,
            Finished,
            Failed
        }

        #endregion infra
    }
}