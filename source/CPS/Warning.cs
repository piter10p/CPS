using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS
{
    public class Warning
    {
        public Exception Exception { get; private set; }
        public string ContentPackFileName { get; private set; }

        public Warning(Exception exception, string contentPackFileName)
        {
            this.Exception = exception;
            this.ContentPackFileName = contentPackFileName;
        }
    }
}
