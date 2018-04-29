using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Porter.Midas.Entities;
using Porter.Midas.Entities.SectionEntities;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasElementEntity
    {
        private string _elemName;
        private string _elemType;
        private string _elemMatID;
        private string _elemPro;
        private double _elemBeta;
        private List<string> _elemNode = new List<string>();
        private string _elemSubType;
        private string _elemWallID;

        public string ElemName { get { return _elemName; } set { _elemName = value; } }
        public string ElemType { get { return _elemType; } set { _elemType = value; } }
        public string ElemMatID { get { return _elemMatID; } set { _elemMatID = value; } }
        public string ElemPro { get { return _elemPro; } set { _elemPro = value; } }
        public double ElemBeta { get { return _elemBeta; } set { _elemBeta = value; } }
        public List<string> ElemNode { get { return _elemNode; } set { _elemNode = value; } }
        public string ElemSubType { get { return _elemSubType; } set { _elemSubType = value; } }
        public string ElemWallID { get { return _elemWallID; } set { _elemWallID = value; } }

        public static Dictionary<string, MidasElementEntity> ReadStrings(StreamReader sr)
        {
            Dictionary<string, MidasElementEntity> result = new Dictionary<string, MidasElementEntity>();
            MidasElementEntity elem;
            List<string> strList;
            string elemID;
            string str = sr.ReadLine();
            int i;

            while (str[0] == ';')
            {
                str = sr.ReadLine();
            }

            while (str != "")
            {
                strList = StringUtility.Split(str, ",");
                elem = new MidasElementEntity();

                elemID = strList[0];
                elem.ElemName = elemID;
                elem.ElemType = strList[1];
                elem.ElemMatID = strList[2];
                elem.ElemPro = strList[3];
                switch (strList[1])
                {
                    case "BEAM":
                        elem.ElemNode.Add(strList[4]);
                        elem.ElemNode.Add(strList[5]);
                        elem.ElemBeta = Convert.ToDouble(strList[6]);
                        if (strList.Count > 7) elem.ElemSubType = strList[7];
                        break;
                    case "WALL":
                        for (i = 1; i <= 4; i++)
                        {
                            elem.ElemNode.Add(strList[i + 3]);
                        }
                        if (strList.Count > 9) elem.ElemSubType = strList[8];
                        if (strList.Count > 10) elem.ElemWallID = strList[9];
                        break;
                    case "PLATE":
                        for (i = 1; i <= 4; i++)
                        {
                            elem.ElemNode.Add(strList[i + 3]);
                        }
                        if (strList.Count > 9) elem.ElemSubType = strList[8];
                        if (strList.Count > 10) elem.ElemWallID = strList[9];
                        break;
                }

                result.Add(elemID, elem);
                str = sr.ReadLine();
            }
            return result;
        }
    }
}
