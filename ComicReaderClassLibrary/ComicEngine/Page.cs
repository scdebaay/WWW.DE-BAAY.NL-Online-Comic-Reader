using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using SkiaSharp;
using System.IO;
using ComicReaderClassLibrary.ImageEngine;

namespace ComicReaderClassLibrary.ComicEngine
{
    public class Page : IComparable<Page>
    {
        private IArchiveEntry entry;
        public Page(IArchiveEntry entry)
        {
            this.entry = entry;
            Paths = new List<SKBitmap>();
        }

        private SKBitmap image;
        public SKBitmap Image
        {
            get
            {
                if ((image == null) && (!IsLoaded))
                {
                    ExtractIfRequired();
                    SKBitmap wb = ResizedBitmap.CreateImage(new SKManagedStream(Bytes), null);
                    if (wb != null)
                    {
                        image = wb;
                    }
                    IsLoaded = true;
                }
                return image;
            }
        }

        private SKBitmap thumbnail;
        public SKBitmap Thumbnail
        {
            get
            {
                ExtractIfRequired();
                if ((thumbnail == null) && (Bytes != null))
                {
                    thumbnail = ResizedBitmap.CreateImage(new SKManagedStream(Bytes), 90);
                }
                return thumbnail;
            }

        }

        public List<SKBitmap> Paths
        {
            get;
            private set;
        }

        private bool isLoaded;
        public bool IsLoaded
        {
            get
            {
                return isLoaded;
            }
            private set
            {
                isLoaded = value;
                //OnPropertyChanged("IsLoaded");
                //OnPropertyChanged("Paths");
            }
        }

        internal void ExtractIfRequired()
        {
            if (Bytes == null)
            {
                Bytes = ExtractBytes();
            }
        }

        /// <summary>
        /// If the size parameter is present, resize the bitmap using the size int as width.
        /// </summary>
        /// <param name="size">Int to be used as the width if the resized bitmap.</param>
        /// <returns></returns>
        public SKBitmap ReSizePage(int size)
        {
            ExtractIfRequired();
            SKBitmap bi = ResizedBitmap.CreateImage(new SKManagedStream(Bytes), size);
            return bi;
        }

        //Moved resizing into ImageEngine namespace.

        internal void RefreshImage()
        {
            //OnPropertyChanged("Image");
            //OnPropertyChanged("IsLoaded");
            //OnPropertyChanged("Thumbnail");
        }

        private Stream Bytes
        {
            get;
            set;
        }

        public int CompareTo(Page other)
        {
            return Name.CompareTo(other.Name);
        }

        public bool HasError
        {
            get;
            private set;
        }

        //public string ToolTip
        //{
        //    get
        //    {
        //        if (Exception.IsNullOrEmpty())
        //        {
        //            return Name;
        //        }
        //        else
        //        {
        //            return Name + Environment.NewLine + Exception;
        //        }
        //    }
        //}

        private string Exception
        {
            get;
            set;
        }
        public string Name
        {
            get
            {
                return entry.Key;
            }
        }
        public Stream ExtractBytes()
        {
            MemoryStream output = new MemoryStream();
            entry.WriteTo(output);
            output.Position = 0;
            return output;
        }
    }
}