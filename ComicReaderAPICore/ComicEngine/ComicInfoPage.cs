using System.Collections.Generic;
using SkiaSharp;
using System.Linq;
using System.Xml.Linq;
using ComicReaderAPICore.Resources;

namespace ComicReaderAPICore.ComicEngine
{
    public class ComicInfoPage
    {
        private Page page;

        public ComicInfoPage()
        {
            PathItems = new HashSet<SKBitmap>();
        }

        public int Index { get; private set; }

        public string Uri { get; private set; }

        private ICollection<SKBitmap> PathItems { get; set; }

        public void LoadPage(Page page)
        {
            if (this.page == null)
            {
                this.page = page;
                this.page.Paths.AddRange(PathItems);
            }
        }

        public static ComicInfoPage CreatePage(XElement pageElement)
        {
            var p = new ComicInfoPage();
            var a = pageElement.Attribute("Image");
            if (a != null)
            {
                p.Index = a.Value.ToInt().Value;
            }
            a = pageElement.Attribute("URL");
            if (a != null)
            {
                p.Uri = a.Value;
            }
            IEnumerable<XElement> elements = pageElement.Elements();
            elements.Where(x => x.Name == "Graphics").Elements().Where(x => x.Name == "path")
                .Select(x => x.GetAttributeValue("d"))
                .ForEach(d => { p.PathItems.Add(SKBitmap.Decode(d)); });
            //elements.Where(x => x.Name == "TextData").Elements().Where(x => x.Name == "TextArea")
                //.ForEach(d => { p.PathItems.Add(new TextDrawingItem(new TextArea(d))); });
            return p;
        }
    }
}