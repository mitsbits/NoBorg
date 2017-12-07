using Shouldly;
using System.Globalization;
using Xunit;

namespace Borg.Infra.Tests.ExtensionMethods
{
    public class NumericExtensionsTests
    {
        public NumericExtensionsTests()
        {
            var culture = new CultureInfo("el-GR");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
        }

        [Theory]
        [InlineData(0, "0 B", "", 1)]
        [InlineData(3072, "3 KB", "", 1)]
        [InlineData(3413, "3,3 KB", "", 1)]
        [InlineData(4194304, "4 MB", "", 1)]
        [InlineData(5819680686, "5,4 GB", "", 1)]
        [InlineData(5819680686, "5,42 GB", "", 2)]
        [InlineData(1429365116109, "1,3 TB", "", 1)]
        [InlineData(3072, "KB: 003,000", "{1}: {0:000.000}", 1)]
        public void SizeDisplay(long source, string target, string format, int decimalsToShow)
        {
            source.SizeDisplay(format, decimalsToShow).ShouldBe(target);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 0, 5)]
        [InlineData(3, 5, 5)]
        [InlineData(4, 5, 5)]
        [InlineData(7, 5, 5)]
        [InlineData(9, 10, 5)]
        [InlineData(12, 10, 5)]
        [InlineData(16, 15, 5)]
        [InlineData(25, 0, 50)]
        [InlineData(26, 50, 50)]
        [InlineData(75, 100, 50)]
        [InlineData(76, 100, 50)]
        [InlineData(50, 0, 100)]
        [InlineData(51, 100, 100)]
        [InlineData(149, 100, 100)]
        [InlineData(150, 200, 100)]
        [InlineData(250, 200, 100)]
        [InlineData(251, 300, 100)]
        public void RoundOff(long source, long target, int interval)
        {
            source.RoundOff(interval).ShouldBe(target);
        }

        [Theory]
        [InlineData(0, "0th")]
        [InlineData(1, "1st")]
        [InlineData(2, "2nd")]
        [InlineData(3, "3rd")]
        [InlineData(4, "4th")]
        [InlineData(5, "5th")]
        [InlineData(11, "11th")]
        [InlineData(12, "12th")]
        [InlineData(13, "13th")]
        public void ToOrdinal(int source, string target)
        {
            source.ToOrdinal().ShouldBe(target);
        }
    }
}