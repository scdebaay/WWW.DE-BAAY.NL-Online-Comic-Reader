using ComicReaderClassLibrary.ComicEngine;
using ComicReaderClassLibrary.Models;
using ComicReaderClassLibrary.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace ComicReaderClassLibrary.Tests
{
    public class FolderCrawlerTests
    {
        private readonly FolderCrawler _sut;
        readonly Mock<ILogger<FolderCrawler>> _logger = new Mock<ILogger<FolderCrawler>>();
        readonly Mock<IConfiguration> _config = new Mock<IConfiguration>();
        readonly Mock<IRootModel> _rootModelI = new Mock<IRootModel>();
        Mock<IComic> _comic = new Mock<IComic>();
        IRootModel _rootModel;
        readonly MockFileSystem _fileSystem = new MockFileSystem();

        public FolderCrawlerTests()
        {
            _fileSystem.Directory.CreateDirectory("demo");
            _fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { @"c:\demo\subfolder\Comic01.cbr", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) },
                { @"c:\demo\subfolder2\Comic02.cbz", new MockFileData(new byte[] { 0x12, 0x34, 0x56, 0xd2 }) }
            });
            _comic.SetupGet(e => e.Pages).Returns(new SortableObservableCollection<Page>());
            _sut = new FolderCrawler(_config.Object, _rootModelI.Object, _logger.Object, _comic.Object);
        }

        [Theory]
        [InlineData(@"C:\")]
        public void GetDirectory_ShouldReturnIRootModelFromValidPath(string fileSystemName)
        {
            _rootModel = _sut.GetDirectory(_fileSystem.DirectoryInfo.FromDirectoryName(fileSystemName));
            string expected = @"Comic01.cbr";
            string actual = _rootModel.folder.file[0].name;
            Assert.Equal(expected,actual);
            Assert.Equal(@"\demo\subfolder\Comic01.cbr", _rootModel.folder.file[0].path);
        }

        [Theory]
        [InlineData(@"D:\FolderDoesNotExist\SubFolder")]
        public void GetDirectory_ShouldThrowExceptionOnInvalidPath(string fileSystemName)
        {
            Assert.Throws<ArgumentException>(() => _sut.GetDirectory(_fileSystem.DirectoryInfo.FromDirectoryName(fileSystemName)));
        }
    }
}
