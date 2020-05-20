using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ComicReaderClassLibrary.Models;
using System.IO;
using Microsoft.Extensions.Logging;

namespace ComicReaderAPICore.Controllers
{
    /// <summary>
    /// Image controller, endpoint that returns requested images
    /// </summary>
    [Route("[controller]/{path}/{filename}")]
    [Route("[controller]/{filename}")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ILogger _logger;
        /// <summary>
        /// Contructor, injects FetchedImage model and Logging object
        /// </summary>
        /// <param name="fetchedImage">Image model, representing fetched image containing Result in bytearray format.</param>
        /// <param name="logger">Log object that logs messages to file.</param>
        public ImageController(IFetchedImage fetchedImage, ILogger<ImageController> logger)
        {
            _fetchedImage = fetchedImage;
            _logger = logger;
        }
        private string Path { get; set; }
        private string Filename { get; set; }
        private int? Size { get; set; }
        private readonly IFetchedImage _fetchedImage;

        /// <summary>
        /// Get method for images
        /// </summary>
        /// <param name="path">String, subfolder of the images requested</param>
        /// <param name="filename">String, filename of the image requested</param>
        /// <param name="size">Int, size to return the image in</param>
        /// <returns>Requested image n bytearray format.</returns>
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