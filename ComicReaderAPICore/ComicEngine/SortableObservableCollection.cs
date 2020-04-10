using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ComicReaderAPICore.Resources
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