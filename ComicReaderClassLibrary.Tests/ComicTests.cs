using ComicReaderClassLibrary.ComicEngine;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO.Abstractions;
using Xunit;

namespace ComicReaderClassLibrary.Tests
{
    public class ComicTests
    {
        private readonly Comic _sut;
        readonly Mock<ILogger<Comic>> _logger = new Mock<ILogger<Comic>>();
        readonly IFileSystem _fileSystem = new FileSystem();

        public ComicTests()
        {
            _sut = new Comic(_logger.Object);
        }

        [Theory]
        [InlineData(@".\PhysicalResources\Comics\Smileys.cbr", "Smileys.cbr", 5)]
        [InlineData(@".\PhysicalResources\Comics\Smileys.cbz", "Smileys.cbz", 5)]
        [InlineData(@".\PhysicalResources\Comics\Smileys-Png.cbr", "Smileys-Png.cbr", 5)]
        [InlineData(@".\PhysicalResources\Comics\Smileys-Png.cbz", "Smileys-Png.cbz", 5)]
        public void LoadFromFile_ShouldLoadComic(string requestedFileName, string expected, int expectedTotalPages)
        {
            //Arrange
            IFileInfo info = _fileSystem.FileInfo.FromFileName(requestedFileName);

            //Act            
            Comic actual = _sut.LoadFromFile(info);

            //Assert
            Assert.Equal(expected, actual.Title);
            Assert.Equal(expectedTotalPages, actual.Pages.Count);
        }

        [Theory]
        [InlineData(@".\PhysicalResources\Comics\Smileys1.cbr", "Smileys1.cbr", 0)]
        [InlineData(@".\PhysicalResources\Comics\Smileys1.cbz", "Smileys1.cbz", 0)]
        [InlineData(@".\PhysicalResources\Comics\Smileys1-Png.cbr", "Smileys1-Png.cbr", 0)]
        [InlineData(@".\PhysicalResources\Comics\Smileys1-Png.cbz", "Smileys1-Png.cbz", 0)]
        public void LoadFromFile_ShouldLogErrorOnInvalidComic(string requestedFileName, string expected, int expectedTotalPages)
        {
            //Arrange
            IFileInfo info = _fileSystem.FileInfo.FromFileName(requestedFileName);

            //Act            
            Comic actual = _sut.LoadFromFile(info);

            //Assert
            Assert.Equal(expected, actual.Title);
            Assert.Equal(expectedTotalPages, actual.Pages.Count);
            _logger.Verify(l => l.Log(It.IsAny<LogLevel>(),
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => true),
                        It.IsAny<Exception>(),
                        It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)), Times.Once);
        }
    }
}
