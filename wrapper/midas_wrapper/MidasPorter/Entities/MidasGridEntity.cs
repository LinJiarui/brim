using System;
using System.Collections.Generic;
using System.IO;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasGridEntity 
    {
        private string _gridName;
        private List<double> _xCoord = new List<double>();
        private List<double> _yCoord = new List<double>();

        public string GridName { get { return _gridName; } set { _gridName = value; } }
        public List<double> XCoord { get { return _xCoord; } set { _xCoord = value; } }
        public List<double> YCoord { get { return _yCoord; } set { _yCoord = value; } }

        public static Dictionary<string, MidasGridEntity> ReadStrings(StreamReader sr)
        {
            Dictionary<string, MidasGridEntity> result = new Dictionary<string, MidasGridEntity>();
            MidasGridEntity gr;
            List<string> strList;


            string grID;
            string str = sr.ReadLine();
            while (str[0] == ';')
            {
                str = sr.ReadLine();
            }
            int i;

            while (str != "")
            {
                strList = StringUtility.Split(str);
                gr=new MidasGridEntity();
                grID=strList[0].Substring(5,strList[0].Length-5);
                gr.GridName=grID;
                str=sr.ReadLine();
                strList=StringUtility.Split(str,",");
                strList[0] = strList[0].Substring(2, strList[0].Length - 2);
                for (i = 0; i < strList.Count; i++)
                {
                    gr.XCoord.Add(Convert.ToDouble(strList[i]));
                }
                str = sr.ReadLine();
                strList = StringUtility.Split(str, ",");
                strList[0] = strList[0].Substring(2, strList[0].Length - 2);
                for (i = 0; i < strList.Count; i++)
                {
                    gr.YCoord.Add(Convert.ToDouble(strList[i]));
                }
                result.Add(grID, gr);
                str = sr.ReadLine();

            }
            return result;
        }
    }
}
