using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.Resources
{
    public class SortableObservableCollection<T> : ObservableCollection<T>
    {
        public void Sort()
        {
            Sort(0, Count, null);
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            (this.Items as List<T>).Sort(index, count, comparer);
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}