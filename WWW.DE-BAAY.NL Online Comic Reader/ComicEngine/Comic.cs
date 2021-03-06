﻿using SharpCompress.Archives;
using SharpCompress.Readers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WWW.DE_BAAY.NL_Online_Comic_Reader.Resources;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine
{
    public class Comic : IDisposable
    {
        //Start logger
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private string title;
        private LoadingStrategy fileLoadingStrategy;
        private IArchive archive;

        /// <summary>
        /// The Comic is normally loaded from file. No URL loading is implemented at this time.
        /// A Comic object is created containing the pages available in the archive.
        /// Use the CurrentPage property to create a bitmap using the page parameter.
        /// </summary>
        /// <param name="file">Filepath and file to be loaded.</param>
        /// <returns></returns>
        public static Comic LoadFromFile(FileInfo file)
        {
            return new Comic(file.Name, new FileInfoStrategy(file));
        }

        protected Comic(string name, LoadingStrategy loadingStrategy)
        {
            Pages = new SortableObservableCollection<Page>();
            DeletedPages = new SortableObservableCollection<Page>();
            CurrentIndex = -1;
            title = name;
            fileLoadingStrategy = loadingStrategy;
            fileLoadingStrategy.ComicLoadingCompleted += new EventHandler(fileLoadingStrategy_ComicLoadingCompleted);

            try
            {
                fileLoadingStrategy.BeginLoad();
            }
            catch
            {
                Logger.Error($"There was an error loading the comic {name}");
            }

        }

        void fileLoadingStrategy_ComicLoadingCompleted(object sender, EventArgs e)
        {
            OpenStream(fileLoadingStrategy.Stream);
        }

        #region Loading

        private void OnComicLoadingCompleted()
        {
            if (ComicInfo != null)
            {
                Title = ComicInfo.Title;
                Pages.Sort(0, Pages.Count, ComicInfo);
            }
        }

        private void OpenStream(Stream stream)
        {
            LoadFromStream(stream);

            OnComicLoadingCompleted();
            if (CurrentIndex == -1)
            {
                CurrentIndex++;
                RefreshCurrentPage();
            }
        }

        #endregion

        #region Properties

        public string Title
        {
            get
            {
                return title;
            }
            private set
            {
                title = value;
            }
        }

        public int ViewingIndex
        {
            get
            {
                return index + 1;
            }
        }


        private int index = -1;
        public int CurrentIndex
        {
            get
            {
                return index;
            }
            set
            {
                index = value;
            }
        }


        public SortableObservableCollection<Page> Pages
        {
            get;
            private set;
        }

        public SortableObservableCollection<Page> DeletedPages
        {
            get;
            private set;
        }

        public Page CurrentPage
        {
            get
            {
                if (CurrentIndex >= Pages.Count)
                {
                    CurrentIndex = 0;
                }
                if ((CurrentIndex > -1) && (Pages.Count > 0))
                {
                    return Pages[CurrentIndex];
                }
                return null;
            }
            set
            {
                //never allow to be set to null?
                if (value != null)
                {
                    CurrentIndex = Pages.IndexOf(value);
                }
            }
        }

        internal void RefreshCurrentPage()
        {
            if (CurrentPage != null)
            {
                CurrentPage.RefreshImage();
            }
        }

        #endregion

        private bool isDisposed = false;
        public void Dispose()
        {
            if (!isDisposed)
            {
                if (fileLoadingStrategy != null)
                {
                    fileLoadingStrategy.Dispose();
                    fileLoadingStrategy = null;
                }
            }
            isDisposed = true;
        }

        protected void LoadFromStream(Stream stream)
        {
            List<Page> pages = new List<Page>();
            if (archive != null)
            {
                archive.Dispose();
            }
            archive = ArchiveFactory.Open(stream, new ReaderOptions());
            foreach (var entry in archive.Entries)
            {
                if (!entry.IsDirectory && entry.Key.IsValidExtension())
                {
                    Page page = new Page(entry);
                    pages.Add(page);
                }
            }

            var info = archive.Entries.Where(x => x.Key.Equals("comicinfo.xml",
                    StringComparison.InvariantCultureIgnoreCase)).SingleOrDefault();
            if (info != null)
            {
                MemoryStream ms = new MemoryStream();
                info.WriteTo(ms);
                ms.Seek(0, SeekOrigin.Begin);
                LoadComicInfo(ms);
            }

            pages.Sort();
            Pages.AddRange(pages);

        }
        protected string Extension
        {
            get;
            set;
        }

        protected string Filter
        {
            get;
            set;
        }

        private MemoryStream comicInfoStream;
        protected void LoadComicInfo(Stream xml)
        {
            comicInfoStream = new MemoryStream();
            xml.TransferTo(comicInfoStream);
            comicInfoStream.Position = 0;
        }

        private ComicInfo ci;
        public ComicInfo ComicInfo
        {
            get
            {
                if (ci == null)
                {
                    ci = ComicInfo.Read(comicInfoStream);
                }
                return ci;
            }
        }
    }
}