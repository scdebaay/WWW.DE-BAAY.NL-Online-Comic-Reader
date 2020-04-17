using SkiaSharp;
using System.IO;

namespace ComicReaderClassLibrary.Resources
{
    public class ContentType : IContentType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ContentType()
        {
        }

        string contentType = "";
        /// <summary>
        /// The ContType properties are set based on the file extension
        /// </summary>
        /// <param name="type">The extension of the file being processed</param>
        /// <param name="file">path and filename of which the type is to be determined</param>
        public void SetType(string file)
        {
            Extension = Path.GetExtension(file);
            switch (Extension)
            {
                case ".png":
                case ".PNG":
                    contentType = "Image/png";
                    Type = contentType;
                    Format = SKEncodedImageFormat.Png;
                    FolderType = "ImageFolder";
                    break;
                case ".JPG":
                case ".jpg":
                    contentType = "Image/jpg";
                    Type = contentType;
                    Format = SKEncodedImageFormat.Jpeg;
                    FolderType = "ImageFolder";
                    break;
                case ".GIF":
                case ".gif":
                    contentType = "Image/gif";
                    Type = contentType;
                    Format = SKEncodedImageFormat.Gif;
                    FolderType = "ImageFolder";
                    break;
                case ".CBR":
                case ".cbr":
                    contentType = "Application/x-cbr";
                    Type = contentType;
                    FolderType = "ComicFolder";
                    break;
                case ".CBZ":
                case ".cbz":
                    contentType = "Application/x-cbr";
                    Type = contentType;
                    FolderType = "ComicFolder";
                    break;
                default:
                    contentType = "Text/plain";
                    Type = contentType;
                    FolderType = "FileFolder";
                    break;
            }

        }


        public string Extension { private set; get; }
        public string Type { private set { } get { return contentType; } }
        public SKEncodedImageFormat Format { private set; get; }
        public string FolderType { private set; get; }
    }
}