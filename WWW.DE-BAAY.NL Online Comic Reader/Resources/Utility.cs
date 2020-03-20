using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.Xml;

namespace WWW.DE_BAAY.NL_Online_Comic_Reader.Resources
{
    public static class Utility
    {
        public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> items)
        {
            foreach (IEnumerable<T> group in items)
            {
                foreach (T item in group)
                {
                    yield return item;
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T item in items)
            {
                action(item);
            }
        }

        public static T? ToEnum<T>(this string val)
            where T : struct
        {
            T e;
            if (Enum.TryParse<T>(val, out e))
            {
                return e;
            }
            return null;
        }

        public static int? ToInt(this string val)
        {
            int e;
            if (int.TryParse(val, out e))
            {
                return e;
            }
            return null;
        }

        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> range)
        {
            range.ForEach(item => collection.Add(item));
        }

        public static void TransferTo(this Stream source, Stream destination)
        {
            byte[] buffer = new byte[4096];
            int read;
            while ((read = source.Read(buffer, 0, buffer.Length)) > 0)
            {
                destination.Write(buffer, 0, read);
            }
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsValidExtension(this string path)
        {
            switch (Path.GetExtension(path).ToLowerInvariant())
            {
                case ".png":
                case ".jpg":
                case ".jpeg":
                case ".bmp":
                case ".gif":
                    return true;
                default:
                    return false;
            }
        }


        public static string SingleValue(this IEnumerable<XElement> elements, string node)
        {
            var e = elements.Where(x => x.Name == node).SingleOrDefault();
            if (e == null)
            {
                return null;
            }
            return e.Value;
        }


        public static IEnumerable<string> MultiValue(this IEnumerable<XElement> elements, string node)
        {
            return elements.Where(x => x.Name == node).Select(x => x.Value);
        }

        public static string GetAttributeValue(this XElement element, string attri)
        {
            var attr = element.Attribute(attri);
            if (attr == null)
            {
                return string.Empty;
            }
            return attr.Value;
        }
        public static string GetAttributeValue(this ICollection<XAttribute> attributes, string attri)
        {
            var attr = attributes.SingleOrDefault(x => x.Name == attri);
            if (attr == null)
            {
                return string.Empty;
            }
            return attr.Value;
        }

        public static HttpContext DetermineRequestedFileType(HttpContext context, string file)
        {
            context.Response.ContentType = new ContType(file).ContentType;
            return context;
        }

        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }        
    }
}