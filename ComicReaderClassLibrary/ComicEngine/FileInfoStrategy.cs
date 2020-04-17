using System.IO;

namespace ComicReaderClassLibrary.ComicEngine
{
    public class FileInfoStrategy : LoadingStrategy
    {
        private FileInfo info;

        public FileInfoStrategy(FileInfo info)
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