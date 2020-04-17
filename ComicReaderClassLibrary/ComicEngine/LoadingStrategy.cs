using System;
using System.IO;

namespace ComicReaderClassLibrary.ComicEngine
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
            ComicLoadingCompleted?.Invoke(this, new EventArgs());
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