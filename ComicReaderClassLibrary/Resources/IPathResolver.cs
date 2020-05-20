namespace ComicReaderClassLibrary.Resources
{
    /// <summary>
    /// Interface that defines PathResolver objects with which UNC paths for file retrieval are resolved from relative paths provided.
    /// </summary>
    public interface IPathResolver
    {
        dynamic this[string name] { get; }

        string ComicFolder { get; }
        string DefaultFile { get; }
        string DefaultFileType { get; }
        string EpubFolder { get; }
        string FileFolder { get; }
        string ImageFolder { get; }
        string MediaFolder { get; }
        string MusicFolder { get; }
        int PageLimit { get; }

        /// <summary>
        /// Method to get the Server Path for a specific file object.
        /// </summary>
        /// <param name="requestedFileName">String, representing the relative path for a file for which to retrieve the UNC path</param>
        /// <returns>String, full UNC path from which to retrieve the requested file</returns>
        string ServerPath(string requestedFileName);
        /// <summary>
        /// Checks whether the path provided is valid
        /// </summary>
        /// <param name="path">String, path to validate</param>
        /// <param name="allowRelativePaths">Boolean, set to true if Relative paths to return are allowed.</param>
        /// <returns>Boolean, is true if the path is valid, false if the path is not valid</returns>
        public bool IsValidPath(string path, bool allowRelativePaths = true);
    }
}