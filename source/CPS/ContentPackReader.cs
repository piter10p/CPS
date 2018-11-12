using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Xml;

namespace CPS
{
    public class ContentPackReader
    {
        DirectoryInfo contentPackFolder;
        Queue<Warning> warningsQueue = new Queue<Warning>();

        /// <summary>
        /// Creates a list of unloaded content packs.
        /// </summary>
        /// <param name="contentPacksFolderPath">Content packs folder path.</param>
        /// <returns></returns>
        public List<ContentPack> ReadContentPacks(string contentPacksFolderPath)
        {
            try
            {
                GetContentPackFolder(contentPacksFolderPath);
                List<ContentPack> contentPacks = new List<ContentPack>();

                foreach (FileInfo file in contentPackFolder.GetFiles("*." + FileExtensions.ContentPack))
                {
                    try
                    {
                        contentPacks.Add(ReadCotentPackFile(file));
                    }
                    catch (Exception e)
                    {
                        NewWarning(e, file.Name);
                    }
                }

                return contentPacks;
            }
            catch (Exception e)
            {
                throw new Exception("Loading content packs exception.", e);
            }
        }

        /// <summary>
        /// Returns true, if there's any warnings to read.
        /// </summary>
        public bool WarningsExists
        {
            get
            {
                if (warningsQueue.Count > 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// Reads next warning and deletes it from list.
        /// </summary>
        public Warning NextWarning
        {
            get
            {
                return warningsQueue.Dequeue();
            }
        }

        private void GetContentPackFolder(string contentPacksFolderPath)
        {
            try
            {
                contentPackFolder = new DirectoryInfo(contentPacksFolderPath);
            }
            catch (Exception e)
            {
                throw new Exception("Getting content pack folder exception.", e);
            }
        }

        private ContentPack ReadCotentPackFile(FileInfo file)
        {
            ContentPackData contentPackData = new ContentPackData();

            try
            {
                ZipArchive zipArchive = OpenZipArchive(file);
                ReadPackFile(zipArchive, contentPackData);
                ReadPackEntries(zipArchive, contentPackData);
            }
            catch (Exception e)
            {
                throw new Exception("Problem with reading content pack file.", e);
            }

            return new ContentPack(contentPackData);
        }

        private ZipArchive OpenZipArchive(FileInfo file)
        {
            try
            {
                FileStream zipToOpen = File.OpenRead(file.FullName);
                return new ZipArchive(zipToOpen, ZipArchiveMode.Read);
            }
            catch (Exception e)
            {
                throw new Exception("Problem with decompressing file.", e);
            }
        }

        private void ReadPackFile(ZipArchive zipArchive, ContentPackData contentPackData)
        {
            try
            {
                XmlDocument xml = OpenPackFile(zipArchive);
                XmlNode root = xml.GetElementsByTagName("ContentPack")[0];

                XmlNode node;

                node = root.SelectSingleNode("Name");
                contentPackData.Name = node.InnerText;

                node = root.SelectSingleNode("Id");
                contentPackData.Id = node.InnerText;

                node = root.SelectSingleNode("Version");
                contentPackData.Version = node.InnerText;

                node = root.SelectSingleNode("CPSVersion");
                contentPackData.CPSVersion = node.InnerText;

                node = root.SelectSingleNode("Author");
                contentPackData.Author = node.InnerText;
            }
            catch (Exception e)
            {
                throw new Exception("Problem with reading ContentPack.xml file.", e);
            }
        }

        private void ReadPackEntries(ZipArchive zipArchive, ContentPackData contentPackData)
        {
            try
            {
                foreach (ZipArchiveEntry entry in zipArchive.Entries)
                {
                    if (IsEntryAnInfoFile(entry))
                    {
                        ElementInfoFileReader elementInfoFileReader = new ElementInfoFileReader();
                        ContentPackElementData contentPackElementData = elementInfoFileReader.ReadFile(entry, zipArchive);
                        contentPackData.Elements.Enqueue(contentPackElementData);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Problem with reading pack entries.", e);
            }
        }

        private bool IsEntryAnInfoFile(ZipArchiveEntry entry)
        {
            try
            {
                string fileExtension = entry.Name.Split('.')[1];//gets file extension

                if (fileExtension == FileExtensions.ElementInfoFile)
                    return true;
            }
            catch (Exception e) { }
            return false;
        }

        private XmlDocument OpenPackFile(ZipArchive zipArchive)
        {
            try
            {
                ZipArchiveEntry zipArchiveEntry = zipArchive.GetEntry("Content Pack.xml");
                Stream fileStream = zipArchiveEntry.Open();
                XmlDocument xml = new XmlDocument();
                xml.Load(fileStream);
                return xml;
            }
            catch (Exception e)
            {
                throw new Exception("Problem with opening file.", e);
            }
        }

        private void NewWarning(Exception e, string contentPackFileName)
        {
            Warning warning = new Warning(e, contentPackFileName);
            warningsQueue.Enqueue(warning);
        }
    }
}
