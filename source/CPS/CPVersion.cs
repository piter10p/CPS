using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPS
{
    static class CPVersion
    {
        static uint major = 0;
        static uint minor = 1;

        static string AsString
        {
            get
            {
                string version = major + "." + minor;
                return version;
            }
        }

        static public bool IsCompatible(string version)
        {
            try
            {
                string[] splittedVersion = version.Split('.');
                uint inputMajor = uint.Parse(splittedVersion[0]);
                uint inputMinor = uint.Parse(splittedVersion[1]);

                if (major == inputMajor && minor == inputMinor)
                    return true;
                return false;
            }
            catch(Exception e)
            {
                throw new Exception("Input has bad formatting.", e);
            }
        }
    }
}
