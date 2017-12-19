using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine
{
    public abstract class LoadingStrategy : IDisposable
    {
        public event EventHandler ComicLoadingCompleted;

        public Stream Stream
        {
            get;
            protected set;
        }

        public abstract void BeginLoad();

        protected void OnComicLoadingCompleted()
        {
            if (ComicLoadingCompleted != null)
            {
                ComicLoadingCompleted(this, new EventArgs());
            }
        }

        public void Dispose()
        {
            if (Stream != null)
            {
                Stream.Dispose();
                Stream = null;
            }
        }
    }
}