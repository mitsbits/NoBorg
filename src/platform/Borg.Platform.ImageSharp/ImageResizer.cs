using Borg.Infra.Storage.Assets.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.Primitives;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Borg.Platform.ImageSharp
{
    public class ImageResizer : IImageResizer
    {
        private readonly ILogger _logger;

        public ImageResizer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory == null ? NullLogger.Instance : loggerFactory.CreateLogger(GetType());
        }

        public Task<Stream> ResizeByLargeSide(Stream input, int sizeInPixels, string mime)
        {
            var stream = new MemoryStream();
            try
            {
                Image<Rgba32> source;
                using (input)
                {
                    input.Seek(0, 0);
                    switch (mime)
                    {
                        case "image/jpeg": source = Image.Load(input, new JpegDecoder()); break;
                        case "image/png": source = Image.Load(input, new PngDecoder()); break;
                        case "image/bmp": source = Image.Load(input, new BmpDecoder()); break;
                        case "image/gif": source = Image.Load(input, new GifDecoder()); break;
                        default: throw new InvalidOperationException($"Can not resize image of type {mime}"); break;
                    }
       
                    var isHorizontal = source.Height < source.Width;
                    var ratio = (float)sizeInPixels / (float)(isHorizontal ? source.Width : source.Height);
                    var newWidth = (int)(source.Width * ratio);
                    var newHeight = (int)(source.Height * ratio);
                    source.Mutate(c =>
                    {
                        c.Resize(new Size(newWidth, newHeight));
                    });
                }
                source.SaveAsJpeg(stream);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return Task.FromResult((Stream)stream);
        }
    }
}