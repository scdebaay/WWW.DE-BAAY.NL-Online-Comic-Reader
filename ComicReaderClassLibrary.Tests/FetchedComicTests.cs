using ComicReaderClassLibrary.ComicEngine;
using ComicReaderClassLibrary.Models;
using ComicReaderClassLibrary.Resources;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO.Abstractions;
using Xunit;

namespace ComicReaderClassLibrary.Tests
{
    public class FetchedComicTests
    {

        private readonly Comic _comic;
        private readonly Mock<ILogger<Comic>> _logger = new Mock<ILogger<Comic>>();
        private readonly Mock<IPathResolver> _pathResolver = new Mock<IPathResolver>();
        private readonly Mock<IContentType> _contentType = new Mock<IContentType>();
        private readonly IFileSystem _fileSystem = new FileSystem();
        private readonly FetchedComic _sut;

        public FetchedComicTests()
        {
            _pathResolver.Setup(p => p.ServerPath(It.IsAny<string>())).Returns(@".\PhysicalResources\Comics\Smileys.cbr");
            _pathResolver.Setup(p => p.IsValidPath(It.IsAny<string>(), true)).Returns(true);
            _contentType.Setup(c => c.SetType(It.IsAny<string>()));
            _contentType.SetupGet(c => c.Type).Returns("Image/jpg");
            _comic = new Comic(_logger.Object);
            _sut = new FetchedComic(_pathResolver.Object, _contentType.Object, _fileSystem, _comic);
        }

        [Fact]
        public void LoadComic_ShouldLoadComic()
        {
            _sut.LoadComic("", null, 0);

            string expectedName = "Smileys.cbr";
            string actualName = _sut.Name;
            Assert.Equal(expectedName, actualName);

            int expected = 5;
            int actual = _sut.TotalPages;
            Assert.Equal(expected, actual);

            string expectedType = "Image/jpg";
            string actualType = _sut.PageType;
            Assert.Equal(expectedType, actualType);

            string expectedPageName = "Smiley-01.jpg";
            string actualPageName = _sut.PageName;
            Assert.Equal(expectedPageName, actualPageName);

            string expectedPath = "";
            string actualPath = _sut.Path;
            Assert.Equal(expectedPath, actualPath);
        }
    }
}
