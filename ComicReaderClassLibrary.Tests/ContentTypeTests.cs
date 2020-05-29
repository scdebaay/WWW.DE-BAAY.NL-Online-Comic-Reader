using ComicReaderClassLibrary.Resources;
using SkiaSharp;
using Xunit;

namespace ComicReaderClassLibrary.Tests
{
    public class ContentTypeTests
    {
        private readonly ContentType _sut;
        public ContentTypeTests()
        {
            _sut = new ContentType();
        }

        [Theory]
        [InlineData("\\300\\300.cbz", "ComicFolder", ".cbz", "Application/x-cbr", null)]
        [InlineData("\\Alex\\Alex-01.cbr", "ComicFolder", ".cbr", "Application/x-cbr", null)]
        [InlineData("\\Pinkie.jpg", "ImageFolder", ".jpg", "Image/jpg", SKEncodedImageFormat.Jpeg)]
        [InlineData("\\Notfound.png", "ImageFolder", ".png", "Image/png", SKEncodedImageFormat.Png)]
        public void SetType_ShouldSetCorrectProperties(string file, string expectedFolderType, string expectedExtension, string expectedType, SKEncodedImageFormat expectedFormat )
        {
            _sut.SetType(file);
            Assert.Equal(expectedFolderType, _sut.FolderType);
            Assert.Equal(expectedExtension, _sut.Extension);
            Assert.Equal(expectedType, _sut.Type);
            Assert.Equal(expectedFormat, _sut.Format);
        }
    }
}
