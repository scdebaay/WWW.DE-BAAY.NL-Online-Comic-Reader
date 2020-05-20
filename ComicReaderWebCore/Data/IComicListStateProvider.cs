namespace ComicReaderWebCore.Data
{
    /// <summary>
    /// Interface that defines the Comic List State provider
    /// </summary>
    public interface IComicListStateProvider
    {
        /// <summary>
        /// Int, keeps the current List page for each session on the website 
        /// </summary>
        int CurrentListPage { get; set; }

        /// <summary>
        /// Fired when current List page is changed
        /// </summary>
        event ComicListStateProvider.EventHandler ComicListPageChanged;

        /// <summary>
        /// Eventhandler for when the comic list page was changed.
        /// </summary>
        /// <param name="o">Object, event sender</param>
        /// <param name="e">PageChangeEventArgs, int, representing the page number the list was changed to</param>
        void OnComicListPageChanged(object o, ComicListStateProvider.PageChangeEventArgs e);
    }
}