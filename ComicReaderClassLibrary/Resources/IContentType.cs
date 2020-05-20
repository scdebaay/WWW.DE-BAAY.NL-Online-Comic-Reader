using SkiaSharp;

namespace ComicReaderClassLibrary.Resources
{
    /// <summary>
    /// Class with which to determine and set a specific content type to be used when returning the content over HTTP
    /// </summary>
    public interface IContentType
    {
        string Extension { get; }
        string FolderType { get; }
        SKEncodedImageFormat Format { get; }
        string Type { get; }

        /// <summary>
        /// Method to set the properties of this class to the type of the file provided
        /// </summary>
        /// <param name="file">String, path to the file for which a type needs to be determined.</param>
        void SetType(string file);
    }
}