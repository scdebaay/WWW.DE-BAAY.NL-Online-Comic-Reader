using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ComicReaderClassLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComicReaderAPICore.Controllers
{
    /// <summary>
    /// Version controller, endpoint that returns the API version in JSON format
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        /// <summary>
        /// Get method for Version
        /// </summary>
        /// <returns>Serialized Version model in JSON format</returns>
        [Produces("application/json")]
        [HttpGet]
        public APIVersionModel Get()
        {
            APIVersionModel version = new APIVersionModel()
            {
                APIName = $"{Assembly.GetExecutingAssembly().GetName().Name}",
                APIVersion = $"{Assembly.GetExecutingAssembly().GetName().Version}",
                LibraryName = $"{typeof(APIVersionModel).Assembly.GetName().Name}", 
                LibraryVersion = $"{typeof(APIVersionModel).Assembly.GetName().Version}"
            };
            return version;
        }
    }
}