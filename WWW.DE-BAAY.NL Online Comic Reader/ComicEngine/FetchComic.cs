using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using WWW.DE_BAAY.NL_Online_Comic_Reader.Resources;
using System.Drawing;
using System.Drawing.Imaging;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine
{
    public class FetchComic
    {
        public FetchComic(string comicFile, HttpContext context, ApiConfiguration config)
        {
            this.configuration = config;
            string MediaPath = this.configuration.serverPath + this.configuration.comicFolder + comicFile;
            int? page = context.Request.QueryString["page"].ToInt();
            int? size = context.Request.QueryString["size"].ToInt();
            FileInfo comicToLoad = new FileInfo(MediaPath);
            Comic comic = Comic.LoadFromFile(comicToLoad);
            PageType = new ContType(comic.CurrentPage.Name.ToString()).ContentType;
            PageName = comic.CurrentPage.Name;
            Result(comic, page, size);
        }

        public byte[] Result(Comic comic, int? page, int? size)
        {
            if (page != null)
            {
                comic.CurrentIndex = (int)page;
            }
            Bitmap resizedpage = null;
            if (size != null)
            {
                resizedpage = new Bitmap(comic.CurrentPage.ReSizePage((int)size));
            }
            else
            {
                resizedpage = comic.CurrentPage.Image;
            }

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
            else
            { return null; }
        }
        public string PageName { private set; get; }
        private ApiConfiguration configuration { set; get; }
        public byte[] result { set; get;}
        public string PageType { private set; get; }
    }
}