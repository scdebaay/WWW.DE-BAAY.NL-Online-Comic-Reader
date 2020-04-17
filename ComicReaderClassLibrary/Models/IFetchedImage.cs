using SkiaSharp;

namespace ComicReaderClassLibrary.Models
{
    public interface IFetchedImage
    {
        string ImageType { get; }
        byte[] Result { get; set; }
        public void LoadImage(string imageFile, int? size);
    }
}