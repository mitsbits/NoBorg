using Borg.Infra.Storage;
using Borg.Platform.Azure.Storage.Blobs;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Xunit;

namespace Borg.Platform.Azure.Tests
{
    public class StorageTests
    {
        private readonly byte[] _objData = new byte[]
        {
            123,
            34,
            78,
            117,
            109,
            101,
            114,
            105,
            99,
            34,
            58,
            52,
            50,
            46,
            53,
            44,
            34,
            73,
            100,
            34,
            58,
            34,
            56,
            57,
            98,
            51,
            48,
            50,
            98,
            99,
            45,
            50,
            53,
            101,
            101,
            45,
            52,
            51,
            51,
            51,
            45,
            97,
            102,
            54,
            97,
            45,
            55,
            53,
            102,
            98,
            98,
            55,
            53,
            51,
            52,
            48,
            101,
            99,
            34,
            44,
            34,
            84,
            101,
            120,
            116,
            117,
            97,
            108,
            34,
            58,
            34,
            72,
            105,
            32,
            116,
            104,
            101,
            114,
            101,
            33,
            34,
            125
        };

        private const string _objGuid = "89B302BC-25EE-4333-AF6A-75FBB75340EC";
        private readonly IFileStorage _azureStorage = new AzureFileStorage(Emulator.AzureStorageConnection);


        #region Azure

        [Fact]
        public async Task save_and_retrieve_and_delete_from_azure_file_storage()
        {
            await DeleteContainer();
            var path = "object.json";
            var saved = await _azureStorage.SaveFile(path, new MemoryStream(_objData));
            saved.ShouldBe(true);
            var retrieved = await _azureStorage.GetFileInfo(path);
            retrieved.FullPath.ShouldBe("http://127.0.0.1:10000/devstoreaccount1/storage/object.json");
            retrieved.Name.ShouldBe(path);
            using (var stream = await _azureStorage.GetFileStream(path)) //TODO: wrapp in extension method
            {
                using (var memStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memStream);
                    var bytesAsString = Encoding.UTF8.GetString(memStream.ToArray());
                    var obj = JsonConvert.DeserializeObject<ObjectToSerialize>(bytesAsString);
                    obj.ShouldNotBeNull();
                    obj.Id.ShouldBe(Guid.Parse(_objGuid));
                }
            }
            var deleted = await _azureStorage.DeleteFile(path);
            deleted.ShouldBe(true);
        }

        [Fact]
        public async Task enumurate_fIles_from_azure_file_storage()
        {
            await DeleteContainer();
            var path = "object.json";
            var innerPath = "inner/secondobject.json";
            var saved = await _azureStorage.SaveFile(path, new MemoryStream(_objData));
            saved.ShouldBe(true);
            saved = await _azureStorage.SaveFile(innerPath, new MemoryStream(_objData));
            saved.ShouldBe(true);

            var files = await _azureStorage.GetFileList();
            files.Count().ShouldBe(2);

            var first = files.Single(x => x.Name == path);
            first.FullPath.ShouldBe("http://127.0.0.1:10000/devstoreaccount1/storage/object.json");
            var second = files.Single(x => x.Name == innerPath);
            second.FullPath.ShouldBe("http://127.0.0.1:10000/devstoreaccount1/storage/inner/secondobject.json");

            var deleted = await _azureStorage.DeleteFile(path);
            deleted.ShouldBe(true);
            deleted = await _azureStorage.DeleteFile(innerPath);
            deleted.ShouldBe(true);
        }

        [Fact]
        public async Task save_and_eetrieve_and_delete_from_scoped_azure_file_storage()
        {
            await DeleteContainer();
            var path = "object.json";
            var scope = "scope";
            using (var scoped = _azureStorage.Scope(scope))
            {
                var explct = scoped as IScopedFileStorage;
                explct.ShouldNotBeNull();
                explct.Scope.ShouldBe(scope);
                var saved = await scoped.SaveFile(path, new MemoryStream(_objData));
                saved.ShouldBe(true);
                var retrieved = await scoped.GetFileInfo(path);
                retrieved.Name.ShouldBe(path);
                var fullPath = $"http://127.0.0.1:10000/devstoreaccount1/storage/{scope}/{ path}";
                retrieved.FullPath.ShouldBe(fullPath);
                using (var stream = await scoped.GetFileStream(path))
                {
                    using (var memStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memStream);
                        var bytesAsString = Encoding.UTF8.GetString(memStream.ToArray());
                        var obj = JsonConvert.DeserializeObject<ObjectToSerialize>(bytesAsString);
                        obj.ShouldNotBeNull();
                        obj.Id.ShouldBe(Guid.Parse(_objGuid));
                    }
                }
                var deleted = await scoped.DeleteFile(path);
                deleted.ShouldBe(true);
            }
        }

        [Fact]
        public async Task enumurate_fIles_from_scoped_azure_file_storage()
        {
            await DeleteContainer();
            var path = "object.json";
            var innerPath = "inner/secondobject.json";
            var scope = "scope";

            using (var scoped = _azureStorage.Scope(scope))
            {
                var explct = scoped as IScopedFileStorage;
                explct.ShouldNotBeNull();
                explct.Scope.ShouldBe(scope);

                var saved = await scoped.SaveFile(path, new MemoryStream(_objData));
                saved.ShouldBe(true);
                saved = await scoped.SaveFile(innerPath, new MemoryStream(_objData));
                saved.ShouldBe(true);

                var files = await scoped.GetFileList();
                files.Count().ShouldBe(2);

                var first = files.Single(x => x.Name == path);
                first.FullPath.ShouldBe("http://127.0.0.1:10000/devstoreaccount1/storage/" + scope + "/" + path);

                var second = files.Single(x => x.Name == innerPath);
                second.FullPath.ShouldBe("http://127.0.0.1:10000/devstoreaccount1/storage/" + scope + "/" + innerPath);

                var deleted = await scoped.DeleteFile(path);
                deleted.ShouldBe(true);
                deleted = await scoped.DeleteFile(innerPath);
                deleted.ShouldBe(true);
            }
        }

        [Fact]
        public async Task from_scoped_discover_fIles_created_from_scurce_azure_file_storage()
        {
            await DeleteContainer();
            var path = "object.json";
            var innerPath = "inner/secondobject.json";
            var scope = "scope";

            using (var scoped = _azureStorage.Scope(scope))
            {
                var explct = scoped as IScopedFileStorage;
                explct.ShouldNotBeNull();
                explct.Scope.ShouldBe(scope);

                var saved = await _azureStorage.SaveFile(scope + '/' + path, new MemoryStream(_objData));
                saved.ShouldBe(true);
                var retrieved = await scoped.GetFileInfo(path);
                retrieved.ShouldNotBeNull();
                retrieved.Name.ShouldBe(path);

                saved = await scoped.SaveFile(innerPath, new MemoryStream(_objData));
                saved.ShouldBe(true);

                retrieved = await _azureStorage.GetFileInfo(scope + '/' + innerPath);
                retrieved.ShouldNotBeNull();

                var deleted = await scoped.DeleteFile(path);
                deleted.ShouldBe(true);
                deleted = await scoped.DeleteFile(innerPath);
                deleted.ShouldBe(true);
            }
        }

        #endregion Azure

        private async Task DeleteContainer()
        {
            var account = CloudStorageAccount.Parse(Emulator.AzureStorageConnection);
            var client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("storage");
            await container.DeleteIfExistsAsync();
            await container.CreateIfNotExistsAsync();
        }
    }

    internal class ObjectToSerialize
    {
        public double Numeric { get; set; }
        public Guid Id { get; set; }
        public string Textual { get; set; }
    }
}