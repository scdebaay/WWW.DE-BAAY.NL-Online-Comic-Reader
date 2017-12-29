using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ComicReaderWeb.Models
{
    public class Comic
    {


        public Comic()
        {
            WebConfiguration config = new WebConfiguration(System.Web.HttpContext.Current);
            int defaultComicSizeInt;
            Int32.TryParse(config.defaultComicSize, out defaultComicSizeInt);
            this.defaultsize = defaultComicSizeInt;
            this.page = 0;
            if (defaultsize < size)
            {
                this.size = size;
            }
            else
            {
                this.size = defaultsize;
            }
        }
        private int defaultsize { get; set; }
        public int size { get; set; }
        public int page { get; set; }
        public int Previous { get {
                if (this.page <= 0)
                {
                    return 0;
                }
                else
                {
                    return this.page - 1;
                }
            }
        }
        public int Next { get {
                return this.page + 1;
            }
        }
        public string requestedFolder { get; set; }
        public string requestedFile { get; set; }                    
    }

}
