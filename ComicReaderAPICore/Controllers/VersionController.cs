using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComicReaderAPICore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComicReaderAPICore.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public APIVersionModel Get()
        {
            APIVersionModel version = new APIVersionModel()
            {
                Name = $"{typeof(APIVersionModel).Assembly.GetName().Name}", 
                Version = $"{typeof(APIVersionModel).Assembly.GetName().Version}"
            };
            return version;
        }
    }
}