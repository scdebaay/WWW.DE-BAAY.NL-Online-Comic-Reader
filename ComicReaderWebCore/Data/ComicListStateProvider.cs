using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComicReaderWebCore.Data
{
    public class ComicListStateProvider : IComicListStateProvider
    {
        public int CurrentListPage { get; set; }
        public string SearchText { get; set; }

        public event EventHandler ComicListPageChanged;

        public virtual void OnComicListPageChanged(object o, PageChangeEventArgs e)
        {
            EventHandler handler = ComicListPageChanged;
            handler?.Invoke(this, e);
        }

        public delegate void EventHandler(Object sender, PageChangeEventArgs e);

        public class PageChangeEventArgs : EventArgs
        {
            public int Page { get; set; }
            public string SearchText { get; set; }
        }
    }
}
