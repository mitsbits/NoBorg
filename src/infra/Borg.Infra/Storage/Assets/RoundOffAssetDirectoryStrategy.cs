using Borg.Infra.Storage.Assets.Contracts;
using Borg.Infra.Storage.Contracts;
using System;
using System.Threading.Tasks;

namespace Borg.Infra.Storage.Assets
{
    public delegate string CalculateParentPath(IFileSpec<int> file);

    public class RoundUpAssetDirectoryStrategy : IAssetDirectoryStrategy<int>
    {
        private readonly int _innerThreshold = 50;
        private readonly int _outerThreshold = 500;
        private readonly bool _wrapFileInFolder = true;

        private readonly CalculateParentPath _calculate;

        public RoundUpAssetDirectoryStrategy() : this(50, 500)
        {
        }

        public RoundUpAssetDirectoryStrategy(int innerThreshold, int outerThreshold, bool wrapFileInFolder = true)
        {
            _innerThreshold = innerThreshold;
            _outerThreshold = outerThreshold;
            _wrapFileInFolder = wrapFileInFolder;
            _calculate = CalculateFromThresholds;
        }

        private string CalculateFromThresholds(IFileSpec<int> asset)
        {
            var key = asset.Id;
            var outer = key.RoundDown(_outerThreshold) ;
            var inner = key.RoundDown(_innerThreshold) ;
            if (_wrapFileInFolder)
            {
                return $"{outer}/{inner}/{key}/";
            }
            return $"{outer}/{inner}/";
        }

        public RoundUpAssetDirectoryStrategy(Func<IFileSpec<int>, string> deleg) : this()
        {
            _calculate = x => deleg(x);
        }

        public Task<string> ParentFolder(IFileSpec<int> file)
        {
            return Task.FromResult(_calculate.Invoke(file));
        }
    }
}