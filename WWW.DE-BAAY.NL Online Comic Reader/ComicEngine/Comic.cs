using SharpCompress.Archives;
using SharpCompress.Readers;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WWW.DE_BAAY.NL_Online_Comic_Reader.Resources;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine
{
    public class Comic : IDisposable
    {
        //public event EventHandler ComicLoadingCompleted;

        private string title;
        private LoadingStrategy fileLoadingStrategy;
        private IArchive archive;

        public static Comic LoadFromUri(Uri url)
        {
            return new Comic(url.AbsolutePath, new DownloadStrategy(url));
        }

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
            this.title = name;
            fileLoadingStrategy = loadingStrategy;
            fileLoadingStrategy.ComicLoadingCompleted += new EventHandler(fileLoadingStrategy_ComicLoadingCompleted);

            try
            {
                fileLoadingStrategy.BeginLoad();
            }
            catch
            {

            }

        }

        void fileLoadingStrategy_ComicLoadingCompleted(object sender, EventArgs e)
        {
            OpenStream(fileLoadingStrategy.Stream);
        }

        //void fileLoadingStrategy_ComicLoadingProgress(object sender, ComicLoadingProgressEventArgs e)
        //{
        //    if (ComicLoadingProgress != null)
        //    {
        //        ComicLoadingProgress(this, e);
        //    }
        //}

        public bool CanSaveOriginalFile()
        {
            return (fileLoadingStrategy is DownloadStrategy);
        }

        #region Save/Export
        //public void SaveOriginalFile()
        //{
        //    SaveFileDialog sfd = new SaveFileDialog();
        //    sfd.DefaultExt = Extension;
        //    sfd.Filter = Filter;

        //    if (sfd.ShowDialog() ?? false)
        //    {
        //        fileLoadingStrategy.Stream.Position = 0;
        //        using (Stream writeableStream = sfd.OpenFile())
        //        {
        //            fileLoadingStrategy.Stream.TransferTo(writeableStream);
        //        }
        //    }
        //}

        //public void ExportCBZ(Action completed)
        //{
        //    throw new NotImplementedException();
        //    //SaveFileDialog sfd = new SaveFileDialog()
        //    //{
        //    //    DefaultExt = "cbz",
        //    //    Filter = "CBZ files (*.cbz)|*.cbz|All files (*.*)|*.*",
        //    //};
        //    //if (sfd.ShowDialog() ?? false)
        //    //{
        //    //    if (!CheckUnloaded())
        //    //    {
        //    //        completed();
        //    //        return;
        //    //    }
        //    //    BackgroundWorker bw = new BackgroundWorker();
        //    //    bw.DoWork += (s, a) =>
        //    //    {
        //    //        ZipOutputStream stream = new ZipOutputStream(a.Argument as Stream);
        //    //        try
        //    //        {
        //    //            stream.SetLevel(9);
        //    //            GetValidPagesForExport().ForEach(p =>
        //    //                {
        //    //                    ZipEntry entry = new ZipEntry(p.Name);
        //    //                    entry.CompressionMethod = CompressionMethod.Deflated;
        //    //                    stream.PutNextEntry(entry);
        //    //                    stream.Write(p.Bytes, 0, p.Bytes.Length);
        //    //                });
        //    //        }
        //    //        catch (Exception e)
        //    //        {
        //    //            Utility.Dispatcher.BeginInvoke(() =>
        //    //            {
        //    //                ErrorDialog ed = new ErrorDialog(e.ToString());
        //    //                ed.Show();
        //    //            });
        //    //        }
        //    //        finally
        //    //        {
        //    //            Utility.Dispatcher.BeginInvoke(() =>
        //    //            {
        //    //                stream.Dispose();
        //    //                completed();
        //    //            });
        //    //        }
        //    //    };
        //    //    bw.RunWorkerAsync(sfd.OpenFile());
        //    //}
        //}

        //public void ExportPDF(Action completed)
        //{
        //    SaveFileDialog sfd = new SaveFileDialog()
        //    {
        //        DefaultExt = "pdf",
        //        Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
        //    };
        //    if (sfd.ShowDialog() ?? false)
        //    {
        //        if (!CheckUnloaded())
        //        {
        //            completed();
        //            return;
        //        }
        //        BackgroundWorker bw = new BackgroundWorker();
        //        bw.DoWork += (s, a) =>
        //        {
        //            Stream exportPDF = a.Argument as Stream;
        //            Document pdf = new Document();
        //            try
        //            {
        //                pdf.SetMargins(0, 0, 0, 0);

        //                PdfWriter.GetInstance(pdf, exportPDF);
        //                pdf.Open();

        //                GetValidPagesForExport().ForEach(p =>
        //                {
        //                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(p.Bytes);
        //                    pdf.SetPageSize(new iTextSharp.text.Rectangle(img.ScaledWidth, img.ScaledHeight));
        //                    pdf.NewPage();
        //                    pdf.Add(img);
        //                });
        //            }
        //            catch (Exception e)
        //            {
        //                Utility.Dispatcher.BeginInvoke(() =>
        //                {
        //                    ErrorDialog ed = new ErrorDialog(e.ToString());
        //                    ed.Show();
        //                });
        //            }
        //            finally
        //            {
        //                Utility.Dispatcher.BeginInvoke(() =>
        //                {
        //                    pdf.Close();
        //                    exportPDF.Dispose();
        //                    completed();
        //                });
        //            }
        //        };
        //        bw.RunWorkerAsync(sfd.OpenFile());
        //    }
        //}

        //private IEnumerable<Page> GetValidPagesForExport()
        //{
        //    Pages.ForEach(p => p.ExtractIfRequired());
        //    return Pages.Where(p => !p.HasError);
        //}

        //private bool CheckUnloaded()
        //{
        //    if (Pages.Where(p => p.HasError).Any())
        //    {
        //        if (MessageBox.Show("Pages have image errors.  Continue?", "Continue?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    return true;
        //}

        #endregion

        #region Loading
        //private void OnComicLoadingProgress(string text, int? percent)
        //{
        //    if (ComicLoadingProgress != null)
        //    {
        //        ComicLoadingProgress(this, new ComicLoadingProgressEventArgs()
        //        {
        //            Text = text,
        //            Percent = percent,
        //        });
        //    }
        //}

        private void OnComicLoadingCompleted()
        {
            if (ComicInfo != null)
            {
                Title = ComicInfo.Title;
                Pages.Sort(0, Pages.Count, ComicInfo);
            }
            //if (ComicLoadingCompleted != null)
            //{
            //    ComicLoadingCompleted(this, new EventArgs());
            //}
        }

        private void OpenStream(Stream stream)
        {
            //Utility.Dispatcher.BeginInvoke(() =>
            //{
            //    OnComicLoadingProgress("Reading File", null);
            //});
            LoadFromStream(stream);
            //Utility.Dispatcher.BeginInvoke(() =>
            //{
            OnComicLoadingCompleted();
            if (CurrentIndex == -1)
            {
                CurrentIndex++;
                RefreshCurrentPage();
            }
            //});
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
                //OnPropertyChanged("Title");
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
                //OnPropertyChanged("CurrentIndex");
                //OnPropertyChanged("ViewingIndex");
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
                    //OnPropertyChanged("CurrentPage");
                }
            }
        }

        internal void RefreshCurrentPage()
        {
            if (CurrentPage != null)
            {
                CurrentPage.RefreshImage();
                //OnPropertyChanged("CurrentPage");
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
                //Ignore comicinfo.xml for now
                //MemoryStream ms = new MemoryStream();
                //info.WriteTo(ms);
                //ms.Seek(0, SeekOrigin.Begin);
                //LoadComicInfo(ms);
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