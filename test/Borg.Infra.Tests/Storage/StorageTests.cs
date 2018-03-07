using Borg.Infra.Serializer;
using Borg.Infra.Storage;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Borg.Infra.Storage.Contracts;
using Xunit;

namespace Borg.Infra.Tests.Storage
{
    public class StorageTests
    {
        private readonly IDictionary<string, string> _mappings;

        private const string _objGuid = "89B302BC-25EE-4333-AF6A-75FBB75340EC";
        private const string _objText = "Hi there!";
        private const double _objNumeric = 42.5;

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

        private const string _objDataString = "{\"Numeric\":42.5,\"Id\":\"89b302bc-25ee-4333-af6a-75fbb75340ec\",\"Textual\":\"Hi there!\"}";

        private const string _storagePath = @"C:\Borg.Infra.Test.Storage";
        private readonly IFileStorage _fileStorage = new InMemoryFileStorage();

        private IFileStorage _folderFileStorage;

        private readonly ISerializer _serializer = new JsonNetSerializer();

        public StorageTests()
        {
            _mappings = new SystemMimesProvider().Mappings;
            if (Directory.Exists(_storagePath)) Directory.Delete(_storagePath, true);
        }

        [Fact]
        public void GetMimeTypeFromSystemCollection()
        {
            foreach (var k in _mappings.Keys)
            {
                $"SomeFile{k}".GetMimeType().ShouldBe(_mappings[k]);
            }
        }

        [Fact]
        public void GetMimeTypeNoExtension()
        {
            var test = @"sduc673j";
            test.GetMimeType().ShouldBe("application/octet-stream");
        }

        [Theory]
        [InlineData("txt", "text/plain")]
        [InlineData(".xls", "application/vnd.ms-excel")]
        [InlineData("dll.config", "text/xml")]
        [InlineData(".dll.config", "text/xml")]
        public void GetMimeTypeHandleJustExtension(string source, string mime)
        {
            source.GetMimeType().ShouldBe(mime);
        }

        [Fact]
        public async Task SaveAndRetrieveAndDeleteFromInMemoryFileStorage()
        {
            var path = "object.json";
            var saved = await _fileStorage.SaveFile(path, new MemoryStream(_objData));
            saved.ShouldBe(true);
            var retrieved = await _fileStorage.GetFileInfo(path);
            retrieved.FullPath.ShouldBe(path);
            retrieved.Name.ShouldBe(path);
            using (var stream = await _fileStorage.GetFileStream(path)) //TODO: wrapp in extension method
            {
                using (var memStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memStream);
                    var obj = await _serializer.DeserializeAsync<ObjectToSerialize>(memStream.ToArray());
                    obj.ShouldNotBeNull();
                    obj.Id.ShouldBe(Guid.Parse(_objGuid));
                }
            }
            var deleted = await _fileStorage.DeleteFile(path);
            deleted.ShouldBe(true);
        }

        [Fact]
        public async Task EnumurateFIlesFromInMemoryFileStorage()
        {
            var path = "object.json";
            var innerPath = "inner/secondobject.json";
            var saved = await _fileStorage.SaveFile(path, new MemoryStream(_objData));
            saved.ShouldBe(true);
            saved = await _fileStorage.SaveFile(innerPath, new MemoryStream(_objData));
            saved.ShouldBe(true);

            var files = await _fileStorage.GetFileList();
            files.Count().ShouldBe(2);
            files.First().Name.ShouldBe("object.json");
            files.First().FullPath.ShouldBe("object.json");
            files.Skip(1).First().Name.ShouldBe("secondobject.json");
            files.Skip(1).First().FullPath.ShouldBe("inner/secondobject.json");

            var deleted = await _fileStorage.DeleteFile(path);
            deleted.ShouldBe(true);
            deleted = await _fileStorage.DeleteFile(innerPath);
            deleted.ShouldBe(true);
        }

        [Fact]
        public async Task SaveAndRetrieveAndDeleteFromScopedInMemoryFileStorage()
        {
            var path = "object.json";
            var scope = "scope";
            using (var scoped = _fileStorage.Scope(scope))
            {
                var explct = scoped as IScopedFileStorage;
                explct.ShouldNotBeNull();
                explct.Scope.ShouldBe(scope);
                var saved = await scoped.SaveFile(path, new MemoryStream(_objData));
                saved.ShouldBe(true);
                var retrieved = await scoped.GetFileInfo(path);
                retrieved.Name.ShouldBe(path);
                var fullPath = $"{scope}/{ path}";
                retrieved.FullPath.ShouldBe(fullPath);
                using (var stream = await scoped.GetFileStream(path)) //TODO: wrapp in extension method
                {
                    using (var memStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memStream);
                        var obj = await _serializer.DeserializeAsync<ObjectToSerialize>(memStream.ToArray());
                        obj.ShouldNotBeNull();
                        obj.Id.ShouldBe(Guid.Parse(_objGuid));
                    }
                }
                var deleted = await scoped.DeleteFile(path);
                deleted.ShouldBe(true);
            }
        }

        [Fact]
        public async Task EnumurateFIlesFromScopedInMemoryFileStorage()
        {
            var path = "object.json";
            var innerPath = "inner/secondobject.json";
            var scope = "scope";

            using (var scoped = _fileStorage.Scope(scope))
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
                files.First().Name.ShouldBe("object.json");
                files.First().FullPath.ShouldBe(scope + "/" + "object.json");
                files.Skip(1).First().Name.ShouldBe("secondobject.json");
                files.Skip(1).First().FullPath.ShouldBe(scope + "/" + "inner/secondobject.json");

                var deleted = await scoped.DeleteFile(path);
                deleted.ShouldBe(true);
                deleted = await scoped.DeleteFile(innerPath);
                deleted.ShouldBe(true);
            }
        }

        [Fact]
        public async Task SaveAndRetrieveAndDeleteFromFolderFileStorage()
        {
            if (!Directory.Exists(_storagePath)) Directory.CreateDirectory(_storagePath);
            _folderFileStorage = new FolderFileStorage(_storagePath, null);

            var path = "object.json";
            var saved = await _folderFileStorage.SaveFile(path, new MemoryStream(_objData));
            saved.ShouldBe(true);
            var retrieved = await _folderFileStorage.GetFileInfo(path);
            retrieved.FullPath.ShouldBe(Path.Combine(_storagePath, path));
            retrieved.Name.ShouldBe(path);
            using (var stream = await _folderFileStorage.GetFileStream(path)) //TODO: wrapp in extension method
            {
                using (var memStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memStream);
                    var obj = await _serializer.DeserializeAsync<ObjectToSerialize>(memStream.ToArray());
                    obj.ShouldNotBeNull();
                    obj.Id.ShouldBe(Guid.Parse(_objGuid));
                }
            }
            var deleted = await _folderFileStorage.DeleteFile(path);
            deleted.ShouldBe(true);
            Directory.Delete(_storagePath);
        }

        [Fact]
        public async Task EnumurateFIlesFromFolderFileStorage()
        {
            if (!Directory.Exists(_storagePath)) Directory.CreateDirectory(_storagePath);
            _folderFileStorage = new FolderFileStorage(_storagePath, null);

            var path = "object.json";
            var innerPath = "inner/secondobject.json";
            var saved = await _folderFileStorage.SaveFile(path, new MemoryStream(_objData));
            saved.ShouldBe(true);
            saved = await _folderFileStorage.SaveFile(innerPath, new MemoryStream(_objData));
            saved.ShouldBe(true);

            var files = await _folderFileStorage.GetFileList();
            files.Count().ShouldBe(2);
            files.First().Name.ShouldBe(path);
            files.First().FullPath.ShouldBe(Path.Combine(_storagePath, path));
            files.Skip(1).First().Name.ShouldBe("secondobject.json");
            files.Skip(1).First().FullPath.ShouldBe(Path.Combine(_storagePath, innerPath.Replace("/", @"\")));

            var deleted = await _folderFileStorage.DeleteFile(path);
            deleted.ShouldBe(true);
            deleted = await _folderFileStorage.DeleteFile(innerPath);
            deleted.ShouldBe(true);

            Directory.Delete(_storagePath, true);
        }

        [Fact]
        public async Task SaveAndRetrieveAndDeleteFromScopedFolderFileStorage()
        {
            if (!Directory.Exists(_storagePath)) Directory.CreateDirectory(_storagePath);
            _folderFileStorage = new FolderFileStorage(_storagePath, null);

            var path = "object.json";
            var scope = "scope";
            using (var scoped = _folderFileStorage.Scope(scope))
            {
                var explct = scoped as IScopedFileStorage;
                explct.ShouldNotBeNull();
                explct.Scope.ShouldBe(scope);
                var saved = await scoped.SaveFile(path, new MemoryStream(_objData));
                saved.ShouldBe(true);
                var retrieved = await scoped.GetFileInfo(path);
                retrieved.Name.ShouldBe(path);
                var fullPath = Path.Combine(_storagePath, scope, path);
                retrieved.FullPath.ShouldBe(fullPath);
                using (var stream = await scoped.GetFileStream(path)) //TODO: wrapp in extension method
                {
                    using (var memStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memStream);
                        var obj = await _serializer.DeserializeAsync<ObjectToSerialize>(memStream.ToArray());
                        obj.ShouldNotBeNull();
                        obj.Id.ShouldBe(Guid.Parse(_objGuid));
                    }
                }
                var deleted = await scoped.DeleteFile(path);
                deleted.ShouldBe(true);
            }
            Directory.Delete(_storagePath, true);
        }

        [Fact]
        public async Task EnumurateFIlesFromScopedFolderFileStorage()
        {
            if (!Directory.Exists(_storagePath)) Directory.CreateDirectory(_storagePath);
            _folderFileStorage = new FolderFileStorage(_storagePath, null);

            var path = "object.json";
            var innerPath = "inner/secondobject.json";
            var scope = "scope";

            using (var scoped = _folderFileStorage.Scope(scope))
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
                files.First().Name.ShouldBe(path);
                files.First().FullPath.ShouldBe(Path.Combine(_storagePath, scope, path));
                files.Skip(1).First().Name.ShouldBe("secondobject.json");
                files.Skip(1).First().FullPath.ShouldBe(Path.Combine(_storagePath, scope, innerPath.Replace("/", @"\")));

                var deleted = await scoped.DeleteFile(path);
                deleted.ShouldBe(true);
                deleted = await scoped.DeleteFile(innerPath);
                deleted.ShouldBe(true);
            }
            Directory.Delete(_storagePath, true);
        }
    }
}