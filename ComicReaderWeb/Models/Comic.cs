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
            this.sizeparam = "&size=";
            this.page = 0;
            this.pageparam = "&page=";
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
        public string sizeparam { get; set; }
        public int size { get; set; }
        public int page { get; set; }
        public string pageparam { get; set;}
        public string requestedfile { get; set; }                    
    }

}
