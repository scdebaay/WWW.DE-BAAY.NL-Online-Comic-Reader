using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ImageEngine
{
    public static class ResizedBitmap
    {
        //Start logger
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Create a bitmap that uses an input stream and an int value to create a bitmap byte array where the width is set to the size int
        /// Aspect ratio of the image is repected.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Bitmap CreateImage(Stream stream, int? size)
        {
            try
            {
                Bitmap bi = new Bitmap(stream);
                //If size is passed into the function, set cx to that value. Othewise keep the original width.
                int cx = size ?? bi.Width;
                //Resulting height is calculated based on ratio between current width and cx (new width).
                decimal temp = (decimal)(bi.Height * (cx / (float)bi.Width));
                //Round decimal temp to nearest int resulting in cy.
                int cy = (int)Math.Round(temp);
                //Create new size parameters for the resulting bitmap.
                Size newsize = new Size(cx, cy);
                //Create a new bitmap based on newsize. Fill it with the data from the original bitmap.
                Image result = new Bitmap(bi, newsize);
                using (Graphics g = Graphics.FromImage(result))
                    g.DrawImage(result, 0, 0, cx, cy);
                return (Bitmap)result;

            }
            catch (Exception e)
            {
                Exception = e.ToString();
                //Debug.WriteLine($"Error while processing graphic");
                Logger.Error($"Error while processing graphic");
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