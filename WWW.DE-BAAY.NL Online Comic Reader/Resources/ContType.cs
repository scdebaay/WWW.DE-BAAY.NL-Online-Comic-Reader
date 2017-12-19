using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.Resources
{
    public class ContType
    {
        public ContType(string file)
        {
            this.Extension = Path.GetExtension(file);
            GetType(Extension); 
        }
        
        private void GetType(string type)
        {
            string contentType = "";
            switch (type)
            {
                case ".png":
                case ".PNG":
                    contentType = "Image/png";
                    this.ContentType = contentType;
                    this.Format = ImageFormat.Png;
                    this.FolderType = "imageFolder";
                    break;
                case ".JPG":
                case ".jpg":
                    contentType = "Image/jpg";
                    this.ContentType = contentType;
                    this.Format = ImageFormat.Jpeg;
                    this.FolderType = "imageFolder";
                    break;
                case ".GIF":
                case ".gif":
                    contentType = "Image/gif";
                    this.ContentType = contentType;
                    this.Format = ImageFormat.Gif;
                    this.FolderType = "imageFolder";
                    break;
                case ".CBR":
                case ".cbr":
                    contentType = "Application/x-cbr";
                    this.ContentType = contentType;
                    this.FolderType = "comicFolder";
                    break;
                case ".CBZ":
                case ".cbz":
                    contentType = "Application/x-cbr";
                    this.ContentType = contentType;
                    this.FolderType = "comicFolder";
                    break;
                default:
                    contentType = "Text/plain";
                    this.ContentType = contentType;
                    this.FolderType = "fileFolder";
                    break;
            }

        }


        public string Extension { private set; get; }
        public string ContentType { private set; get; }
        public ImageFormat Format { private set; get; }
        public string FolderType { private set; get; }
    }
}