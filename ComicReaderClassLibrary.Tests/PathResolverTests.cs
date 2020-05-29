using ComicReaderClassLibrary.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.IO.Abstractions;
using Xunit;

namespace ComicReaderClassLibrary.Tests
{
    public class PathResolverTests
    {
        private readonly PathResolver _sut;
        readonly Mock<ILogger<PathResolver>> _logger = new Mock<ILogger<PathResolver>>();
        readonly Mock<IConfiguration> _config = new Mock<IConfiguration>();        
        readonly Mock<IWebHostEnvironment> _env = new Mock<IWebHostEnvironment>();
        readonly Mock<IContentType> _contentType = new Mock<IContentType>();
        readonly string _currentPath = Directory.GetCurrentDirectory();

        public PathResolverTests()
        {            
            _config.Setup(c => c.GetSection(It.IsAny<String>())).Returns(new Mock<IConfigurationSection>().Object);
            _config.SetupGet(c => c[It.Is<String>(s => s == "ComicFolder")]).Returns("\\\\DE-BAAY.NL\\DistributedFileShare\\Comics\\pub\\");
            _config.SetupGet(c => c[It.Is<String>(s => s == "ImageFolder")]).Returns("Images\\");
            _env.SetupGet(e =>e.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            _sut = new PathResolver(_config.Object, _logger.Object, _env.Object, _contentType.Object);            
        }

        [Theory]
        [InlineData("\\300\\300.cbz", "")]
        public void ServerPath_ShouldResolveEmptyApplicationServerPath(string requestedFileName, string expected)
        {
            //Arrange
            _contentType.SetupGet<string>(x => x.Extension).Returns(".cbz");
            _contentType.SetupGet<string>(x => x.FolderType).Returns("ComicFolder");

            //Act            
            string actual = _sut.ServerPath(requestedFileName);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("\\Pienkie.jpg", "")]
        public void ServerPath_ShouldResolveApplicationServerPath(string requestedFileName, string expected)
        {
            //Arrange
            _contentType.SetupGet<string>(x => x.Extension).Returns(".jpg");
            _contentType.SetupGet<string>(x => x.FolderType).Returns("ImageFolder");
            expected = $"{_currentPath}\\";

            //Act            
            string actual = _sut.ServerPath(requestedFileName);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("\\300\\300.cbz", "\\\\DE-BAAY.NL\\DistributedFileShare\\Comics\\pub\\")]
        public void ServerPath_ShouldResolveCurrentFolder(string requestedFileName, string expected)
        {
            //Arrange
            _contentType.SetupGet<string>(x => x.Extension).Returns(".cbz");
            _contentType.SetupGet<string>(x => x.FolderType).Returns("ComicFolder");
            _sut.ServerPath(requestedFileName);
            //Act            
            string actual = _sut[_contentType.Object.FolderType];

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("\\\\DE-BAAY.NL\\DistributedFileShare\\Comics\\pub\\300\\300.cbz", true)]
        public void IsValidPath_ShouldReturnTrueforValidPath(string requestedFileNamePath, bool expected)
        {
            //Arrange
            _contentType.SetupGet<string>(x => x.Extension).Returns(".cbz");
            _contentType.SetupGet<string>(x => x.FolderType).Returns("ComicFolder");
            //Act            
            bool actual = _sut.IsValidPath(requestedFileNamePath);

            //Assert
            Assert.True(expected);
        }

        [Theory]
        [InlineData("\\\\DE-BAAY.NL\\DistributedFileShare\\Comics\\pub\\300\\300.cbr", true)]
        public void IsValidPath_ShouldReturnFalseforInvalidPath(string requestedFileNamePath, bool expected)
        {
            //Arrange
            _contentType.SetupGet<string>(x => x.Extension).Returns(".cbr");
            _contentType.SetupGet<string>(x => x.FolderType).Returns("ComicFolder");
            //Act            
            bool actual = _sut.IsValidPath(requestedFileNamePath);

            //Assert
            Assert.True(expected);
        }
    }
}
