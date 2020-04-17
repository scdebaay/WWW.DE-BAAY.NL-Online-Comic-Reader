using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using ComicReaderClassLibrary.Resources;

namespace ComicReaderClassLibrary.ComicEngine
{
    public class ComicInfo : IComparer<Page>
    {
        public ComicInfo()
        {
            Writers = new HashSet<string>();
            Pages = new List<ComicInfoPage>();
        }

        public static ComicInfo Read(Stream xml)
        {
            if (xml == null)
            {
                return null;
            }
            var elements = XElement.Load(xml);
            ComicInfo info = new ComicInfo();
            info.Title = elements.Elements().SingleValue("Title");
            info.Writers.AddRange(elements.Elements().MultiValue("Writer"));
            var type = elements.Elements().SingleValue("Type");
            if (!type.IsNullOrEmpty())
            {
                info.ComicInfoType = type.ToEnum<ComicInfoType>().Value;
            }
            elements.Elements().Where(x => x.Name == "Pages").Single()
                .Elements().ForEach(page =>
                {
                    info.Pages.Add(ComicInfoPage.CreatePage(page));
                });
            return info;
        }

        public string Title
        {
            get;
            private set;
        }

        public ICollection<string> Writers
        {
            get;
            private set;
        }

        public ComicInfoType ComicInfoType
        {
            get;
            private set;
        }

        public IList<ComicInfoPage> Pages
        {
            get;
            private set;
        }

        public int Compare(Page x, Page y)
        {
            var xCip = Pages.Where(cip => cip.Uri == x.Name).SingleOrDefault();
            var yCip = Pages.Where(cip => cip.Uri == y.Name).SingleOrDefault();

            if ((xCip == null) && (yCip == null))
            {
                return x.CompareTo(y);
            }
            if (xCip == null)
            {
                return -1;
            }
            if (yCip == null)
            {
                return 1;
            }
            xCip.LoadPage(x);
            yCip.LoadPage(y);
            return xCip.Index.CompareTo(yCip.Index);
        }
    }

    public enum ComicInfoType
    {
        FileComic,
        WebComic,
    }
}