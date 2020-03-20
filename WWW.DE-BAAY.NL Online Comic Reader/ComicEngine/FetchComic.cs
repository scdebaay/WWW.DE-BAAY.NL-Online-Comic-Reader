using System.Drawing;
using System.IO;
using System.Web;
using WWW.DE_BAAY.NL_Online_Comic_Reader.Resources;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine
{
    public class FetchComic
    {
        /// <summary>
        /// A Comic was requested. A FetchComic object is created with a byte array property Result containing the requested page.
        /// </summary>
        /// <param name="comicFile">The file requested, this only contains a relative path.</param>
        /// <param name="context">HttpContext object containing further Querystring paramters</param>
        public FetchComic(string comicFile, HttpContext context)
        {
            string MediaPath = ApiConfiguration.ServerPath(comicFile) + ApiConfiguration.ComicFolder + comicFile;
            int? page = context.Request.QueryString["page"].ToInt();
            int? size = context.Request.QueryString["size"].ToInt();
            FileInfo comicToLoad = new FileInfo(MediaPath);
            //The Comic is loaded.
            Comic comic = Comic.LoadFromFile(comicToLoad);
            PageType = new ContType(comic.CurrentPage.Name.ToString()).ContentType;
            PageName = comic.CurrentPage.Name;
            //The result is compiled based in the comic object, the requested page and size.
            Result(comic, page, size);
        }

        public byte[] Result(Comic comic, int? page, int? size)
        {
            //Page parameter is set, set the CurrentIndex property to the page number.
            if (page != null)
            {
                comic.CurrentIndex = (int)page;
            }
            //Create an empty bitmap to load the current page into.
            Bitmap resizedpage = null;
            //Size is set, process the extracted image before sending it back to the response.
            if (size != null)
            {
                resizedpage = new Bitmap(comic.CurrentPage.ReSizePage((int)size));
            }
            //No processing is needed, request the resulting image for the CurrentPage.
            else
            {
                resizedpage = comic.CurrentPage.Image;
            }
            //All processing to get a bitmap to be disaplyed completed. Return the resulting image as a byte array.
            if (comic.CurrentPage != null)
            {
                using (Bitmap image = new Bitmap(resizedpage))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, new ContType(comic.CurrentPage.Name).Format);
                        ms.Position = 0;
                        result = ms.ToArray();
                        ms.Close();
                        return result;
                    }
                }
            }
            //There is no resulting bitmap. Return nothing.
            else
            { return null; }
        }
        public string PageName { private set; get; }
        public byte[] result { set; get;}
        public string PageType { private set; get; }
    }
}