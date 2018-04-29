using System.Collections.Generic;
using System.IO;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasVersionEntity
    {
        private string _version;

        public string Version { get { return _version; } set { _version = value; } }

        public void ReadStrings(StreamReader sr)
        {
            string str = sr.ReadLine();
            List<string> versions = StringUtility.Split(str);
            _version = versions[0];
        }
    }
}
