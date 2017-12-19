using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using System.Xml.Linq;
using WWW.DE_BAAY.NL_Online_Comic_Reader.Resources;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.ComicEngine
{
    public class ComicInfoPage
    {
        private Page page;

        public ComicInfoPage()
        {
            PathItems = new HashSet<Bitmap>();
        }

        public int Index { get; private set; }

        public string Uri { get; private set; }

        private ICollection<Bitmap> PathItems { get; set; }

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
                .ForEach(d => { p.PathItems.Add(new Bitmap(d)); });
            //elements.Where(x => x.Name == "TextData").Elements().Where(x => x.Name == "TextArea")
                //.ForEach(d => { p.PathItems.Add(new TextDrawingItem(new TextArea(d))); });
            return p;
        }
    }
}