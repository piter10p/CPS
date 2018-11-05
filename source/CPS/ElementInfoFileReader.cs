using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Xml;
using System.IO;

namespace CPS
{
    class ElementInfoFileReader
    {
        XmlDocument xmlDocument;
        ContentPackElementData elementData = new ContentPackElementData();

        public ContentPackElementData ReadFile(ZipArchiveEntry entry, ZipArchive zipArchive)
        {
            try
            {
                OpenDocument(entry);

                elementData.zipEntry = GetContentFile(entry, zipArchive);

                XmlNode root = xmlDocument.GetElementsByTagName("ContentPackElement")[0];
                elementData.Name = root.SelectSingleNode("Name").InnerText;
                elementData.Id = root.SelectSingleNode("Id").InnerText;
                elementData.Type = root.SelectSingleNode("Type").InnerText;

                return elementData;
            }
            catch (Exception e)
            {
                throw new Exception("Problem with reading element info file: " + entry.Name, e);
            }
        }

        private void OpenDocument(ZipArchiveEntry entry)
        {
            try
            {
                Stream fileStream = entry.Open();
                xmlDocument = new XmlDocument();
                xmlDocument.Load(fileStream);
            }
            catch (Exception e)
            {
                throw new Exception("Problem with opening file.", e);
            }
        }

        private ZipArchiveEntry GetContentFile(ZipArchiveEntry entry, ZipArchive zipArchive)
        {
            try
            {
                string nameWithoutExtension = entry.FullName.Split(' ')[0];

                return zipArchive.GetEntry(nameWithoutExtension);
            }
            catch (Exception e)
            {
                throw new Exception("Problem with finding content file entry.", e);
            }
        }
    }
}
