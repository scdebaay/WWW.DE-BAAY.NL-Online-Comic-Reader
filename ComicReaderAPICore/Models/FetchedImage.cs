using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComicReaderAPICore.Resources;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using SkiaSharp;
using ComicReaderAPICore.ImageEngine;

namespace ComicReaderAPICore.Models
{
    public class FetchedImage : IFetchedImage
    {
        readonly IPathResolver _pathResolver;
        readonly IContentType _contentType;
        string MediaPath = string.Empty;
        SKBitmap originalImage;
        public FetchedImage(IPathResolver pathresolver, IContentType contentType)
        {
            _pathResolver = pathresolver;
            _contentType = contentType;
        }

        public void LoadImage(string imageFile, int? size)
        {
            MediaPath = $"{_pathResolver.ServerPath(imageFile)}{ _pathResolver.ImageFolder}{imageFile}";
            if (System.IO.File.Exists(MediaPath))
            {
                _contentType.SetType(imageFile);
                ImageType = _contentType.Type;
            }
            else
            {
                MediaPath = $"{_pathResolver.ServerPath(_pathResolver.DefaultFile)}{ _pathResolver.ImageFolder}{_pathResolver.DefaultFile}";
                _contentType.SetType(_pathResolver.DefaultFile);
                ImageType = _contentType.Type;
            }
            
            originalImage = SKBitmap.Decode(MediaPath);            
            SetResult(size);

        }

        private byte[] SetResult(int? size)
        {
            SKBitmap resizedimage;

            if (size != null)
            {
                resizedimage = ResizedBitmap.CreateImage(MediaPath, (int)size);
            }
            else
            {
                resizedimage = originalImage;
            }

            using MemoryStream ms = new MemoryStream();
            resizedimage.Encode(ms, ImageFormat, 100);
            ms.Position = 0;
            Result = ms.ToArray();
            ms.Close();
            return Result;


        }
        public byte[] Result { set; get; }
        public string ImageType { get; private set; }
        private SKEncodedImageFormat ImageFormat
        {
            get
            {
                SKData imgInput = SKData.Create(MediaPath);
                SKEncodedImageFormat thisFormat = SKCodec.Create(imgInput).EncodedFormat;
                return thisFormat;
            }
        }
    }
}