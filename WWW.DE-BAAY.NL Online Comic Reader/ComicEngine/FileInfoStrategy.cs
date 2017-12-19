using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WWW.DE_BAAY.NL_Online_Comic_Reader.Resources;
using System.IO;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine
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