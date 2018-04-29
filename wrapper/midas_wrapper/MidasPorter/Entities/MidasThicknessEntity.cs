using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasThicknessEntity
    {
        private string _thickNumber;
        private string _thickType;
        private bool _bsame;
        private double _thickIn;
        private double _thickOut;
        private int _thickId;

        public string ThickNumber { get { return _thickNumber; } set { _thickNumber = value; } }
        public string ThickType { get { return _thickType; } set { _thickType = value; } }
        public bool Bsame { get { return _bsame; } set { _bsame = value; } }
        public double ThickIn { get { return _thickIn; } set { _thickIn = value; } }
        public double ThickOut { get { return _thickOut; } set { _thickOut = value; } }
        public int ThickId { get { return _thickId; } set { _thickId = value; } }

        public static Dictionary<string, MidasThicknessEntity> ReadStrings(StreamReader sr)
        {
            Dictionary<string, MidasThicknessEntity> result = new Dictionary<string, MidasThicknessEntity>();
            MidasThicknessEntity thick;
            List<string> strList;
            string thickID;
            string str = sr.ReadLine();
            while (str[0] == ';')
            {
                str = sr.ReadLine();
            }
            while (str != "")
            {
                strList = StringUtility.Split(str, ",");
                thick = new MidasThicknessEntity();
                thickID = strList[0];
                thick.ThickNumber = thickID;
                thick.ThickType = strList[1];
                thick.Bsame = (strList[2] == "YES" ? true : false);
                thick.ThickIn = Convert.ToDouble(strList[3]);
                thick.ThickOut = Convert.ToDouble(strList[4]);
                result.Add(thickID,thick);
                str = sr.ReadLine();
            }
            return result;
        }
        
    }
}
