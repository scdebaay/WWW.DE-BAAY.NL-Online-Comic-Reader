using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WWW.DE_BAAY.NL_Online_Comic_Reader.Resources;
using System.Net;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine
{
    public class DownloadStrategy : LoadingStrategy
    {
        private Uri url;
        public DownloadStrategy(Uri url)
        {
            this.url = url;
        }

        public override void BeginLoad()
        {
            //Utility.Dispatcher.BeginInvoke(() =>
            //{
                //OnComicLoadingProgress("Downloading", null);
            //});
            try
            {
                WebClient downloader = new WebClient();
                downloader.OpenReadCompleted += new OpenReadCompletedEventHandler(downloader_OpenReadCompleted);
                downloader.DownloadProgressChanged += new DownloadProgressChangedEventHandler(downloader_DownloadProgressChanged);
                downloader.OpenReadAsync(url);
            }
            catch (Exception e)
            {
                //MessageBox.Show("Not supported or Bad url: " + url);
                //ErrorDialog ed = new ErrorDialog(e.ToString());
                //ed.Show();
            }
        }

        void downloader_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //Utility.Dispatcher.BeginInvoke(() =>
            //{
            //    OnComicLoadingProgress("Downloading: " + e.BytesReceived + " / " + e.TotalBytesToReceive, e.ProgressPercentage);
            //});
        }

        void downloader_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            Stream = e.Result;
            OnComicLoadingCompleted();
        }
    }
}