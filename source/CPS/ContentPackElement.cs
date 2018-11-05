using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace CPS
{
    public class ContentPackElement
    {
        public string Name { get; private set; }
        public string Id { get; private set; }
        public string Type { get; private set; }
        public ContentPack Parent { get; private set; }

        private ZipArchiveEntry targetFileZipEntry;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="name">Name of element</param>
        /// <param name="id">ID of element</param>
        /// <param name="type">Type of element</param>
        public ContentPackElement(ContentPackElementData data, ContentPack parent)
        {
            this.Name = data.Name;
            this.Id = data.Id;
            this.Type = data.Type;
            this.Parent = parent;
            this.targetFileZipEntry = data.targetFileZipEntry;
        }

        public Stream OpenStream()
        {
            try
            {
                return targetFileZipEntry.Open();
            }
            catch (Exception e)
            {
                throw new Exception("Problem with opening stream.", e);
            }
        }
    }
}
