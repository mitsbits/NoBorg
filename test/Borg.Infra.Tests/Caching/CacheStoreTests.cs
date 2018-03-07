using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Borg.Infra.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Shouldly;
using Xunit;

namespace Borg.Infra.Tests.Caching
{
    public class CacheStoreTests
    {
        private readonly CacheStore _cache;
        private readonly string _testString = "A bunch of characters";
        private readonly a_point_with_a_name _testStruct;

        public CacheStoreTests()
        {

            _cache = new CacheStore(
                new MemoryDistributedCache(
                    new OptionsWrapper<MemoryDistributedCacheOptions>(new MemoryDistributedCacheOptions())),
                new JsonNetSerializer(), NullLoggerFactory.Instance);
   
            _testStruct = new a_point_with_a_name(1, 42, _testString);
        }

        [Fact]
        public async Task read_and_write_a_string()
        {
            var key = Guid.NewGuid().ToString();
            await _cache.Set(key, _testString);
            var read = await _cache.Get<string>(key);
            read.ShouldBe(_testString);
        }

        [Fact]
        public async Task read_and_write_a_struct()
        {
            var key = Guid.NewGuid().ToString();
            await _cache.Set(key, _testStruct);
            var read = await _cache.Get<a_point_with_a_name>(key);
            read.x.ShouldBe(_testStruct.x);
            read.y.ShouldBe(_testStruct.y);
            read.name.ShouldBe(_testStruct.name);
        }



        internal struct a_point_with_a_name
        {
            internal a_point_with_a_name(int x, int y, string name)
            {
                this.x = x;
                this.y = y;
                this.name = name;

            }

            public int x { get; set; }
            public int y { get; set; }
            public string name { get; set; }
        }
    }
}
