using SkiaSharp;

namespace ComicReaderAPICore.Resources
{
    public interface IContentType
    {
        string Extension { get; }
        string FolderType { get; }
        SKEncodedImageFormat Format { get; }
        string Type { get; }

        void SetType(string file);
    }
}