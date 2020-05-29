using ComicReaderClassLibrary.ComicEngine;
using ComicReaderClassLibrary.ImageEngine;
using SharpCompress.Archives;
using SharpCompress.Readers;
using SkiaSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace ComicReaderClassLibrary.Tests
{
    public class ResizedBitmapTests
    {
        private Stream _stream;
        private Assembly _assembly = Assembly.GetExecutingAssembly();
        private SKManagedStream _sKStream;
        public ResizedBitmapTests()
        {

        }

        [Theory]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-01.jpg", @".\PhysicalResources\Images\Smiley-01.jpg")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-02.jpg", @".\PhysicalResources\Images\Smiley-02.jpg")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-03.jpg", @".\PhysicalResources\Images\Smiley-03.jpg")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-04.jpg", @".\PhysicalResources\Images\Smiley-04.jpg")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-05.jpg", @".\PhysicalResources\Images\Smiley-05.jpg")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-01.png", @".\PhysicalResources\Images\Smiley-01.png")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-02.png", @".\PhysicalResources\Images\Smiley-02.png")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-03.png", @".\PhysicalResources\Images\Smiley-03.png")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-04.png", @".\PhysicalResources\Images\Smiley-04.png")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-05.png", @".\PhysicalResources\Images\Smiley-05.png")]
        public void CreateImage_FromStream_ShouldReturnSKImage(string actualBitmapPath, string expectedBitmapPath)
        {
            SKBitmap expectedBitmap = SKBitmap.Decode(expectedBitmapPath);
            _stream = _assembly.GetManifestResourceStream(actualBitmapPath);
            _sKStream = new SKManagedStream(_stream);
            SKBitmap actualBitmap = ResizedBitmap.CreateImage(_sKStream, null);
            int expected = expectedBitmap.Width;
            int actual = actualBitmap.Width;
            Assert.Equal(expected, actual);
            expected = expectedBitmap.Height;
            actual = actualBitmap.Height;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-01.jpg", @".\PhysicalResources\Images\Smiley-01.jpg")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-02.jpg", @".\PhysicalResources\Images\Smiley-02.jpg")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-03.jpg", @".\PhysicalResources\Images\Smiley-03.jpg")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-04.jpg", @".\PhysicalResources\Images\Smiley-04.jpg")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-05.jpg", @".\PhysicalResources\Images\Smiley-05.jpg")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-01.png", @".\PhysicalResources\Images\Smiley-01.png")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-02.png", @".\PhysicalResources\Images\Smiley-02.png")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-03.png", @".\PhysicalResources\Images\Smiley-03.png")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-04.png", @".\PhysicalResources\Images\Smiley-04.png")]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-05.png", @".\PhysicalResources\Images\Smiley-05.png")]
        public void CreateImage_FromFile_ShouldReturnSKImage(string expectedBitmapPath, string actualBitmapPath)
        {
            _stream = _assembly.GetManifestResourceStream(expectedBitmapPath);
            _sKStream = new SKManagedStream(_stream);
            SKBitmap expectedBitmap = SKBitmap.Decode(_stream);
            SKBitmap actualBitmap = ResizedBitmap.CreateImage(actualBitmapPath, null);
            int expected = expectedBitmap.Width;
            int actual = actualBitmap.Width;
            Assert.Equal(expected, actual);
            expected = expectedBitmap.Height;
            actual = actualBitmap.Height;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-01.jpg", @".\PhysicalResources\Images\Smiley-01.jpg", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-02.jpg", @".\PhysicalResources\Images\Smiley-02.jpg", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-03.jpg", @".\PhysicalResources\Images\Smiley-03.jpg", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-04.jpg", @".\PhysicalResources\Images\Smiley-04.jpg", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-05.jpg", @".\PhysicalResources\Images\Smiley-05.jpg", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-01.png", @".\PhysicalResources\Images\Smiley-01.png", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-02.png", @".\PhysicalResources\Images\Smiley-02.png", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-03.png", @".\PhysicalResources\Images\Smiley-03.png", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-04.png", @".\PhysicalResources\Images\Smiley-04.png", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-05.png", @".\PhysicalResources\Images\Smiley-05.png", 150)]
        public void ResizeImage_FromStream_ShouldReturnSKImage(string actualBitmapPath, string expectedBitmapPath, int size)
        {
            SKBitmap expectedBitmap = SKBitmap.Decode(expectedBitmapPath);
            _stream = _assembly.GetManifestResourceStream(actualBitmapPath);
            _sKStream = new SKManagedStream(_stream);
            SKBitmap actualBitmap = ResizedBitmap.CreateImage(_sKStream, size);
            int expected = size;
            int actual = actualBitmap.Width;
            Assert.Equal(expected, actual);
            expected = (int)Math.Round((decimal)(expectedBitmap.Height * (size / (float)expectedBitmap.Width)));
            actual = actualBitmap.Height;
            Assert.Equal(expected, actual);
        }
        [Theory]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-01.jpg", @".\PhysicalResources\Images\Smiley-01.jpg", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-02.jpg", @".\PhysicalResources\Images\Smiley-02.jpg", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-03.jpg", @".\PhysicalResources\Images\Smiley-03.jpg", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-04.jpg", @".\PhysicalResources\Images\Smiley-04.jpg", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-05.jpg", @".\PhysicalResources\Images\Smiley-05.jpg", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-01.png", @".\PhysicalResources\Images\Smiley-01.png", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-02.png", @".\PhysicalResources\Images\Smiley-02.png", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-03.png", @".\PhysicalResources\Images\Smiley-03.png", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-04.png", @".\PhysicalResources\Images\Smiley-04.png", 150)]
        [InlineData("ComicReaderClassLibrary.Tests.PhysicalResources.Images.Smiley-05.png", @".\PhysicalResources\Images\Smiley-05.png", 150)]
        public void ResizeImage_FromFile_ShouldReturnSKImage(string expectedBitmapPath, string actualBitmapPath, int size)
        {
            _stream = _assembly.GetManifestResourceStream(expectedBitmapPath);
            _sKStream = new SKManagedStream(_stream);
            SKBitmap expectedBitmap = SKBitmap.Decode(_stream);
            SKBitmap actualBitmap = ResizedBitmap.CreateImage(actualBitmapPath, size);
            int expected = size;
            int actual = actualBitmap.Width;
            Assert.Equal(expected, actual);
            expected = (int)Math.Round((decimal)(expectedBitmap.Height * (size / (float)expectedBitmap.Width)));
            actual = actualBitmap.Height;
            Assert.Equal(expected, actual);
        }

    }
}
