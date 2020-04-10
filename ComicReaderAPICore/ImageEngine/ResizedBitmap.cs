using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using SkiaSharp;
using NLog;

namespace ComicReaderAPICore.ImageEngine
{
    public static class ResizedBitmap
    {
        //Start logger
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Create a bitmap that uses an input stream and an int value to create a bitmap byte array where the width is set to the size int
        /// Aspect ratio of the image is repected.
        /// </summary>
        /// <param name="stream">filestream to be resized</param>
        /// <param name="size">size in width or height to resize to</param>
        /// <returns></returns>
        public static SKBitmap CreateImage(SKManagedStream stream, int? size)
        {
            try
            {
                SKBitmap resized = Resize(stream, size);
                return resized;
            }
            catch (Exception e)
            {
                _logger.Error($"Error while resizing graphic: {e.Message}");
                _logger.Error($"Stacktrace: {e.StackTrace}");
                HasError = true;
                return null;
            }
        }

        /// <summary>
        /// Create a bitmap that uses an input stream and an int value to create a bitmap byte array where the width is set to the size int
        /// Aspect ratio of the image is repected.
        /// </summary>
        /// <param name="path">path to file to be resized</param>
        /// <param name="size">size in width or height to resize to</param>
        /// <returns></returns>
        public static SKBitmap CreateImage(string path, int? size)
        {
            try
            {
                SKBitmap resized =  Resize(path, size);
                return resized;
            }
            catch (Exception e)
            {
                _logger.Error($"Error while resizing graphic from file: {e.Message}");
                _logger.Error($"File under proces: {path}");
                _logger.Error($"Stacktrace: {e.StackTrace}");
                HasError = true;
                return null;
            }
        }

        /// <summary>
        /// Create a bitmap that uses an input stream and an int value to create a bitmap byte array where the width is set to the size int
        /// Aspect ratio of the image is repected.
        /// </summary>
        /// <param name="original">path to file or stream to be resized</param>
        /// <param name="size">size in width or height to resize to</param>
        private static SKBitmap Resize(dynamic original, int? size)
        {
            try
            {
                SKBitmap bi = SKBitmap.Decode(original);
                //If size is passed into the function, set cx to that value. Othewise keep the original width.
                int cx = size ?? bi.Width;
                //Resulting height is calculated based on ratio between current width and cx (new width).
                decimal temp = (decimal)(bi.Height * (cx / (float)bi.Width));
                //Round decimal temp to nearest int resulting in cy.
                int cy = (int)Math.Round(temp);
                //Create new size parameters for the resulting bitmap.
                SKImageInfo newsize = new SKImageInfo(cx, cy);
                //Create a new bitmap based on newsize. Fill it with the data from the original bitmap.
                var resized = bi.Resize(newsize, SKFilterQuality.Medium);
                return resized;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static bool HasError
        {
            get;
            private set;
        }
    }
}