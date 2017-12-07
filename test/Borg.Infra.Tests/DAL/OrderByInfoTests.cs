using Borg.Infra.DAL;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace Borg.Infra.Tests.DAL
{
    public class OrderByInfoTests
    {
        [Fact]
        public void CheckDirectiveConstructorWithoutOrder()
        {
            Should.NotThrow(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, Guid>("Id");
                info.Property.Body.Type.ShouldBe(typeof(Guid));
                info.ToString().ShouldBe($"{typeof(ObjectToSerialize).Name}.Id|" + "ASC");
            });
            Should.NotThrow(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, double>("Numeric:");
                info.Property.Body.Type.ShouldBe(typeof(double));
                info.ToString().ShouldBe($"{typeof(ObjectToSerialize).Name}.Numeric|" + "ASC");
            });
            Should.Throw<ArgumentException>(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, object>("NoSuchProperty");
            });
        }

        [Fact]
        public void CheckDirectiveConstructorWithOrder()
        {
            Should.NotThrow(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, Guid>("Id:DESC");
                info.Property.Body.Type.ShouldBe(typeof(Guid));
                info.ToString().ShouldBe($"{typeof(ObjectToSerialize).Name}.Id|" + "DESC");
            });
            Should.NotThrow(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, double>("Numeric:whatever");
                info.Property.Body.Type.ShouldBe(typeof(double));
                info.ToString().ShouldBe($"{typeof(ObjectToSerialize).Name}.Numeric|" + "ASC");
            });
            Should.Throw<ArgumentException>(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, object>("NoSuchProperty:ASC");
            });
        }

        [Fact]
        public void CheckPropertyConstructorWithoutOrder()
        {
            Should.NotThrow(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, Guid>(x => x.Id);
                info.Property.Body.Type.ShouldBe(typeof(Guid));
                info.ToString().ShouldBe($"{typeof(ObjectToSerialize).Name}.Id|" + "ASC");
            });
            Should.NotThrow(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, double>(x => x.Numeric);
                info.Property.Body.Type.ShouldBe(typeof(double));
                info.ToString().ShouldBe($"{typeof(ObjectToSerialize).Name}.Numeric|" + "ASC");
            });
            Should.Throw<ArgumentException>(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, object>(x => x);
            });
        }

        [Fact]
        public void CheckPropertyConstructorWithOrder()
        {
            Should.NotThrow(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, Guid>(x => x.Id, false);
                info.Property.Body.Type.ShouldBe(typeof(Guid));
                info.ToString().ShouldBe($"{typeof(ObjectToSerialize).Name}.Id|" + "DESC");
            });
            Should.NotThrow(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, double>(x => x.Numeric, true);
                info.Property.Body.Type.ShouldBe(typeof(double));
                info.ToString().ShouldBe($"{typeof(ObjectToSerialize).Name}.Numeric|" + "ASC");
            });
            Should.Throw<ArgumentException>(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize, object>(x => x, true);
            });
        }

        [Fact]
        public void CheckDynamicPRopertyConstructorWithOrder()
        {
            Should.NotThrow(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize>(x => x.Id, false);
                info.Property.Body.Type.ShouldBe(typeof(object));
                info.ToString().ShouldBe($"{typeof(ObjectToSerialize).Name}.Id|" + "DESC");
            });
            Should.NotThrow(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize>(x => x.Numeric);
                info.Property.Body.Type.ShouldBe(typeof(object));
                info.ToString().ShouldBe($"{typeof(ObjectToSerialize).Name}.Numeric|" + "ASC");
            });
            Should.Throw<ArgumentException>(() =>
            {
                var info = new OrderByInfo<ObjectToSerialize>(x => new { x });
            });
        }

        [Fact]
        public void CheckSortBuilder()
        {
            Should.NotThrow(() =>
            {
                var orderBys = SortBuilder.Get<ObjectToSerialize>(x => x.Id, false).Add("Textual:DESC").Add(x => x.Numeric, false).Build();
                orderBys.ShouldNotBeNull();
                orderBys.Count().ShouldBe(3);
            });
        }
    }
}