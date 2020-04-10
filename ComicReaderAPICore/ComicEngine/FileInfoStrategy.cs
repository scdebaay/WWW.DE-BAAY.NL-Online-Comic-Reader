using System.IO;

namespace ComicReaderAPICore.ComicEngine
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