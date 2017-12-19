using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.IO;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ImageEngine
{
    public static class ResizedBitmap
    {
        public static Bitmap CreateImage(Stream stream, int? size)
        {
            try
            {
                Bitmap bi = new Bitmap(stream);
                int cx = size ?? bi.Width;
                decimal temp = (decimal)(bi.Height * (cx / (float)bi.Width));
                int cy = (int)Math.Round(temp);

                Size newsize = new Size(cx, cy);
                Image result = new Bitmap(bi, newsize);
                using (Graphics g = Graphics.FromImage(result))
                    g.DrawImage(result, 0, 0, cx, cy);
                return (Bitmap)result;

            }
            catch (Exception e)
            {
                Exception = e.ToString();
                HasError = true;
                return null;
            }
        }

        public static bool HasError
        {
            get;
            private set;
        }

        private static string Exception
        {
            get;
            set;
        }
    }
}