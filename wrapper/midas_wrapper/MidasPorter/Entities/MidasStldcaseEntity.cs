using System.Collections.Generic;
using System.IO;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasStldcaseEntity 
    {
        private string _lcname;
        private string _lctype;
        private string _desc;

        public string Lcname { get { return _lcname; } set { _lcname = value; } }
        public string Lctype { get { return _lctype; } set { _lctype = value; } }
        public string Desc { get { return _desc; } set { _desc = value; } }

        public static Dictionary<string, MidasStldcaseEntity> ReadStrings(StreamReader sr)
        {
            Dictionary<string, MidasStldcaseEntity> result = new Dictionary<string, MidasStldcaseEntity>();
            MidasStldcaseEntity stldcase;
            List<string> strList;
            string stldcaseID;
            string str = sr.ReadLine();

            while (str[0] == ';')
            {
                str = sr.ReadLine();
            }

            while (str != "")
            {
                strList = StringUtility.Split(str, ",");
                stldcase = new MidasStldcaseEntity();
                stldcaseID = strList[0];
                stldcase.Lcname = stldcaseID;
                stldcase.Lctype = strList[1];
                if (strList.Count>2) stldcase.Desc = strList[2];
                result.Add(stldcaseID, stldcase);
                str = sr.ReadLine();
            }
            return result;
        }
    }
}
