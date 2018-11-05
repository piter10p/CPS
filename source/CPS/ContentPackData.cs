using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS
{
    public class ContentPackData
    {
        public string Name;
        public string Id;
        public string Version;
        public string Author;
        public string CPSVersion;

        public Queue<ContentPackElementData> Elements = new Queue<ContentPackElementData>();
    }
}
