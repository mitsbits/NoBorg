using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace Borg.Infra.Tests.ExtensionMethods
{
    public class DictionaryExtensionsTests
    {
        private readonly Dictionary<string, string> _a;
        private readonly Dictionary<string, string> _b;

        public DictionaryExtensionsTests()
        {
            _a = new Dictionary<string, string>() { { "A", "One" }, { "B", "Two" }, { "C", "Three" }, { "D", "Four" } };
            _b = new Dictionary<string, string>() { { "A", "Another One" }, { "E", "Five" }, { "F", "Six" }, { "G", "Seven" } };
        }

        [Fact]
        public void AppendOnly()
        {
            _a.AppendOnly(_b);
            _a.Count.ShouldBe(7);
            _a["A"].ShouldBe("One");
            _a["B"].ShouldBe("Two");
            _a["C"].ShouldBe("Three");
            _a["D"].ShouldBe("Four");
            _a["E"].ShouldBe("Five");
            _a["F"].ShouldBe("Six");
            _a["G"].ShouldBe("Seven");
        }

        [Fact]
        public void AppendAndUpdate()
        {
            _a.AppendAndUpdate(_b);
            _a.Count.ShouldBe(7);
            _a["A"].ShouldBe("Another One");
            _a["B"].ShouldBe("Two");
            _a["C"].ShouldBe("Three");
            _a["D"].ShouldBe("Four");
            _a["E"].ShouldBe("Five");
            _a["F"].ShouldBe("Six");
            _a["G"].ShouldBe("Seven");
        }

        [Fact]
        public void ReplaceAll()
        {
            _a.ReplaceAll(_b);
            _a.Count.ShouldBe(4);
            _a["A"].ShouldBe("Another One");
            _a["E"].ShouldBe("Five");
            _a["F"].ShouldBe("Six");
            _a["G"].ShouldBe("Seven");
        }
    }
}