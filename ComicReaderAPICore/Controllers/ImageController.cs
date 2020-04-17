using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComicReaderClassLibrary.Models;
using System.IO;

namespace ComicReaderAPICore.Controllers
{
    [Route("[controller]/{path}/{filename}")]
    [Route("[controller]/{filename}")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public ImageController(IFetchedImage fetchedImage)
        {
            _fetchedImage = fetchedImage;
        }
        private string Path { get; set; }
        private string Filename { get; set; }
        private int? Size { get; set; }
        private readonly IFetchedImage _fetchedImage;

        [Produces("image/png", "image/jpg", "image/gif")]
        [HttpGet]
        public IActionResult Get(string path, string filename, int? size)
        {
            Path = path;
            Filename = filename;
            Size = size;

            string imageFile;
            if (Path != null)
            {
               imageFile  = $"{Path}\\{Filename}";
            }
            else
            {
                imageFile = Filename;
            } 
            
            _fetchedImage.LoadImage(imageFile, size);

            var stream = _fetchedImage.Result;
            return File(stream, _fetchedImage.ImageType);
        }
    }
}