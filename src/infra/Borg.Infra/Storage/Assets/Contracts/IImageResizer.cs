using System.IO;
using System.Threading.Tasks;

namespace Borg.Infra.Storage.Assets.Contracts
{
    public interface IImageResizer
    {
        Task<Stream> ResizeByLargeSide(Stream input, int sizeInPixels, string mime);
    }
}