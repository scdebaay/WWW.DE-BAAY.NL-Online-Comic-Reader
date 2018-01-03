using System;

namespace ComicReaderWeb.Models
{
    public class Comic
    {


        public Comic()
        {
            //Initialize Comic object by calling the current configuration. We need the default size set in the configuration.
            WebConfiguration config = new WebConfiguration(System.Web.HttpContext.Current);

            //If current value for size property is higher than the defaultSize from the config, size property is set to the value input.
            if (defaultsize < size)
            {
                this.size = size;
            }
            else //The size is smaller than the default value and default value takes precedence.
            {
                this.size = defaultsize;
            }
            //Initialize integer to parse the defaultSize into.
            int defaultComicSizeInt;
            Int32.TryParse(config.defaultComicSize, out defaultComicSizeInt);
            this.defaultsize = defaultComicSizeInt;
            //Initialize page property to 0. Can be set externally.
            this.page = 0;
        }
        private int defaultsize { get; set; }
        public int size { get; set; }
        public int page { get; set; }
        //Derive Previous property from page and totalPages property. No page lower than 1.
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
        //Derive Next property from page. There is no way of determining the total number of pages in a comic yet.
        public int Next { get {
                return this.page + 1;
            }
        }
        public string requestedFolder { get; set; }
        public string requestedFile { get; set; }
        public string ComicName { get {
                return requestedFile.Substring(0, requestedFile.Length - 4);
            }
        }
    }

}
