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

                elementData.targetFileZipEntry = GetContentFile(entry, zipArchive);

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
                string nameWithoutExtension = entry.Name.Split('.')[0];

                foreach(ZipArchiveEntry targetEntry in zipArchive.Entries)
                {
                    string targetNameWithoutExtension = targetEntry.Name.Split('.')[0];

                    if (targetNameWithoutExtension == nameWithoutExtension && IsFileNotAnInformationFile(targetEntry.Name))
                        return targetEntry;
                }

                throw new Exception("Content file not exists.");
            }
            catch (Exception e)
            {
                throw new Exception("Problem with finding content file entry.", e);
            }
        }

        private bool IsFileNotAnInformationFile(string fileName)
        {
            string extension = fileName.Split('.')[1];

            if (extension == FileExtensions.ElementInfoFile)
                return false;
            return true;
        }
    }
}
