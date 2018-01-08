using Borg.Infra.Storage.Assets.Contracts;
using System;
using System.Threading.Tasks;

namespace Borg.Infra.Storage.Assets
{
    public delegate string CalculateParentPath(IAssetInfo<int> asset);

    public class RoundOffAssetDirectoryStrategy : IAssetDirectoryStrategy<int>
    {
        private readonly int _innerThreshold = 50;
        private readonly int _outerThreshold = 500;
        private readonly bool _wrapFileInFolder = true;

        private readonly CalculateParentPath _calculate;

        public RoundOffAssetDirectoryStrategy() : this(50, 500)
        {
        }

        public RoundOffAssetDirectoryStrategy(int innerThreshold, int outerThreshold, bool wrapFileInFolder = true)
        {
            _innerThreshold = innerThreshold;
            _outerThreshold = outerThreshold;
            _wrapFileInFolder = wrapFileInFolder;
            _calculate = CalculateFromThresholds;
        }

        private string CalculateFromThresholds(IAssetInfo<int> asset)
        {
            var key = asset.Id;
            var outer = key.RoundOff(500) + 500;
            var inner = key.RoundOff(_innerThreshold) + _innerThreshold;
            if (_wrapFileInFolder)
            {
                return $"{outer}/{inner}/{key}/";
            }
            return $"{outer}/{inner}/";
        }

        public RoundOffAssetDirectoryStrategy(Func<IAssetInfo<int>, string> deleg) : this()
        {
            _calculate = x => deleg(x);
        }

        public Task<string> ParentFolder(IAssetInfo<int> asset)
        {
            return Task.FromResult(_calculate.Invoke(asset));
        }
    }
}