using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace CPS
{
    public class ContentPackElementData
    {
        public string Name;
        public string Id;
        public string Type;
        public ZipArchiveEntry zipEntry;
    }
}
