using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using WWW.DE_BAAY.NL_Online_Comic_Reader.ImageEngine;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine
{
    public class Page : IComparable<Page>
    {
        private IArchiveEntry entry;
        public Page(IArchiveEntry entry)
        {
            this.entry = entry;
            Paths = new List<Bitmap>();
        }

        private Bitmap image;
        public Bitmap Image
        {
            get
            {
                if ((image == null) && (!IsLoaded))
                {
                    ExtractIfRequired();
                    Bitmap wb = ResizedBitmap.CreateImage(new MemoryStream(Bytes), null);
                    if (wb != null)
                    {
                        image = wb;
                    }
                    IsLoaded = true;
                }
                return image;
            }
        }

        private Bitmap thumbnail;
        public Bitmap Thumbnail
        {
            get
            {
                ExtractIfRequired();
                if ((thumbnail == null) && (Bytes != null))
                {
                    thumbnail = ResizedBitmap.CreateImage(new MemoryStream(Bytes), 90);
                }
                return thumbnail;
            }

        }

        public List<Bitmap> Paths
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
        public Bitmap ReSizePage(int size)
        {
            ExtractIfRequired();
            Bitmap bi = ResizedBitmap.CreateImage(new MemoryStream(Bytes), size);
            return bi;
        }

        //Moved resizing into ImageEngine namespace.

        internal void RefreshImage()
        {
            //OnPropertyChanged("Image");
            //OnPropertyChanged("IsLoaded");
            //OnPropertyChanged("Thumbnail");
        }

        public byte[] Bytes
        {
            get;
            private set;
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
        public byte[] ExtractBytes()
        {
            MemoryStream output = new MemoryStream();
            entry.WriteTo(output);
            output.Position = 0;
            return output.ToArray();
        }
    }
}