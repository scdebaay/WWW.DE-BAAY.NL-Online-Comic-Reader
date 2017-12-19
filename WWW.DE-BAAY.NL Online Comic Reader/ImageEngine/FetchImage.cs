using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WWW.DE_BAAY.NL_Online_Comic_Reader.Resources;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ImageEngine
{
    public class FetchImage
    {
        string MediaPath = String.Empty;
        public FetchImage(string imageFile, HttpContext context, ApiConfiguration config)
        {
            this.configuration = config;
            MediaPath = this.configuration.serverPath + this.configuration.imageFolder + imageFile;
            int? size = context.Request.QueryString["size"].ToInt();
            ImageType = new ContType(imageFile).ContentType;
            Bitmap image = new Bitmap(MediaPath);
            Result(image, size);
            image.Dispose();
        }

        public byte[] Result(Bitmap image, int? size)
        {
            Bitmap resizedimage = null;
            if (size != null)
            {
                FileStream originalimage = File.OpenRead(MediaPath);
                MemoryStream memStream = new MemoryStream();
                memStream.SetLength(originalimage.Length);
                originalimage.Read(memStream.GetBuffer(), 0, (int)originalimage.Length);
                resizedimage = ResizedBitmap.CreateImage(originalimage, (int)size);
                originalimage.Dispose();
            }
            else
            {
                resizedimage = image;
            }

            using (Bitmap imageResult = new Bitmap(resizedimage))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    imageResult.Save(ms, this.ImageFormat);
                    ms.Position = 0;
                    result = ms.ToArray();
                    ms.Close();
                    return result;
                }
            }

        }
        private ApiConfiguration configuration { set; get; }
        public byte[] result { set; get; }
        public string ImageType { get; private set; }
        private ImageFormat ImageFormat
        {
            get
            {
                Image imgInput = Image.FromFile(MediaPath);
                ImageFormat thisFormat = imgInput.RawFormat;
                return thisFormat;
            }
        }
    }
}