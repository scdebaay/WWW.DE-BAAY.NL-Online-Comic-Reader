using ComicReaderClassLibrary.ComicEngine;
using ComicReaderClassLibrary.Resources;
using SkiaSharp;
using System.IO;

namespace ComicReaderClassLibrary.Models
{
    public class FetchedComic : IFetchedComic
    {
        readonly IPathResolver _pathResolver;
        readonly IContentType _contentType;
        public FetchedComic(IPathResolver pathResolver, IContentType contentType)
        {
            _pathResolver = pathResolver;
            _contentType = contentType;
        }
        
        /// <summary>
        /// A Comic was requested. A FetchComic object is created with a byte array property Result containing the requested page.
        /// </summary>
        /// <param name="comicFile">The file requested, this only contains a relative path.</param>
        /// <param name="context">HttpContext object containing further Querystring paramters</param>
        public void LoadComic(string comicFile, int? size, int page = 0)
        {
            string MediaPath = $"{_pathResolver.ServerPath(comicFile)}{_pathResolver.ComicFolder}{comicFile}";
            if (_pathResolver.IsValidPath(MediaPath))
            {
                FileInfo comicToLoad = new FileInfo(MediaPath);
                //The Comic is loaded.
                Comic comic = Comic.LoadFromFile(comicToLoad);
                comic.CurrentIndex = page;
                _contentType.SetType(comic.CurrentPage.Name.ToString());
                PageType = _contentType.Type;
                PageName = comic.CurrentPage.Name;
                Path = comicFile;
                Name = comic.Title;
                TotalPages = comic.Pages.Count;
                //The result is compiled based in the comic object, the requested page and size.
                result(comic, page, size);
                comic.Dispose();
            }
            else
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    SKBitmap defaultImage;
                    defaultImage = SKBitmap.Decode(_pathResolver.DefaultFile);
                    defaultImage.Encode(ms, ImageFormat, 100);
                    _contentType.SetType(_pathResolver.DefaultFile);
                    PageType = _contentType.Type;
                    ms.Position = 0;
                    Result = ms.ToArray();
                    ms.Close();
                }
            }
            
        }

        private byte[] result(Comic comic, int? page, int? size)
        {
            //Page parameter is set, set the CurrentIndex property to the page number.
            if (page != null)
            {
                comic.CurrentIndex = (int)page;
            }
            //Create an empty bitmap to load the current page into.
            SKBitmap resizedpage;
            //Size is set, process the extracted image before sending it back to the response.
            if (size != null)
            {
                resizedpage = comic.CurrentPage.ReSizePage((int)size);
            }
            //No processing is needed, request the resulting image for the CurrentPage.
            else
            {
                resizedpage = comic.CurrentPage.Image;
            }
            //All processing to get a bitmap to be disaplyed completed. Return the resulting image as a byte array.
            if (comic.CurrentPage != null)
            {                
                using (MemoryStream ms = new MemoryStream())
                {
                    resizedpage.Encode(ms, _contentType.Format, 100);
                    ms.Position = 0;
                    Result = ms.ToArray();
                    ms.Close();
                    return Result;
                }
            }
            //There is no resulting bitmap. Return nothing.
            else
            { return null; }
        }

        private SKEncodedImageFormat ImageFormat
        {
            get
            {
                SKData imgInput = SKData.Create(_pathResolver.DefaultFile);
                SKEncodedImageFormat thisFormat = SKCodec.Create(imgInput).EncodedFormat;
                return thisFormat;
            }
        }

        public string Path { private set; get; }
        public string Name { private set; get; }
        public int TotalPages { private set; get; }
        public string PageName { private set; get; }
        public byte[] Result { private set; get; }
        public string PageType { private set; get; }
    }
}