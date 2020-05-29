using ComicReaderClassLibrary.ComicEngine;
using ComicReaderClassLibrary.ImageEngine;
using SharpCompress.Archives;
using SharpCompress.Readers;
using SkiaSharp;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace ComicReaderClassLibrary.Tests
{
    public class PageTests
    {
        private Stream _stream;
        private readonly Stream _imageStreamJpg;
        private readonly SKBitmap _imageBitmapJpg;
        private readonly Stream _imageStreamPng;
        private readonly SKBitmap _imageBitmapPng;
        private Page _sut;
        private IArchive _archive;
        private Assembly _assembly = Assembly.GetExecutingAssembly();

        public PageTests()
        {
            var imagePathJpg = "ComicReaderClassLibrary.Tests.Resources.Images.Smiley-01.jpg";
            _imageStreamJpg = _assembly.GetManifestResourceStream(imagePathJpg);
            _imageBitmapJpg = ResizedBitmap.CreateImage(new SKManagedStream(_imageStreamJpg), null);
            var imagePathPng = "ComicReaderClassLibrary.Tests.Resources.Images.Smiley-01.png";
            _imageStreamPng = _assembly.GetManifestResourceStream(imagePathPng);
            _imageBitmapPng = ResizedBitmap.CreateImage(new SKManagedStream(_imageStreamPng), null);
        }

        [Theory]
        [InlineData("ComicReaderClassLibrary.Tests.Resources.Comics.Smileys.cbr", "Smiley-01.jpg")]
        [InlineData("ComicReaderClassLibrary.Tests.Resources.Comics.Smileys.cbz", "Smiley-01.jpg")]
        public void ImageProperty_ShouldBeValidJpgSKBitmap(string mediaPath, string expectedFileName)
        {
            //Arrange
            _stream = _assembly.GetManifestResourceStream(mediaPath);            
            _archive = ArchiveFactory.Open(_stream, new ReaderOptions());
            
            //Act
            _sut = new Page(_archive.Entries.First());
            string actualFileName = _sut.Name;

            //Assert
            var expected = _imageBitmapJpg;
            var actual = _sut.Image;
            Assert.Equal(expected.ByteCount, actual.ByteCount);
            Assert.Equal(expected.Height, actual.Height);
            Assert.Equal(expected.Width, actual.Width);
            Assert.Equal(expected.AlphaType, actual.AlphaType);
            Assert.Equal(expectedFileName, actualFileName);
        }

        [Theory]
        [InlineData("ComicReaderClassLibrary.Tests.Resources.Comics.Smileys-Png.cbr", "Smiley-01.png")]
        [InlineData("ComicReaderClassLibrary.Tests.Resources.Comics.Smileys-Png.cbz", "Smiley-01.png")]
        public void ImageProperty_ShouldBeValidPngSKBitmap(string mediaPath, string expectedFileName)
        {
            //Arrange
            _stream = _assembly.GetManifestResourceStream(mediaPath);
            _archive = ArchiveFactory.Open(_stream, new ReaderOptions());

            //Act
            _sut = new Page(_archive.Entries.First());
            string actualFileName = _sut.Name;

            //Assert
            var expected = _imageBitmapPng;
            var actual = _sut.Image;
            Assert.Equal(expected.ByteCount, actual.ByteCount);
            Assert.Equal(expected.Height, actual.Height);
            Assert.Equal(expected.Width, actual.Width);
            Assert.Equal(expected.AlphaType, actual.AlphaType);
            Assert.Equal(expectedFileName, actualFileName);
        }
    }
}
