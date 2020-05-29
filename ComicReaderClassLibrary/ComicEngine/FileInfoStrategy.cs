using System.IO;
using System.IO.Abstractions;

namespace ComicReaderClassLibrary.ComicEngine
{
    public class FileInfoStrategy : LoadingStrategy
    {
        private IFileInfo info;

        public FileInfoStrategy(IFileInfo info)
        {
            this.info = info;
        }

        public override void BeginLoad()
        {
            Stream = info.OpenRead();
            OnComicLoadingCompleted();
        }
    }
}