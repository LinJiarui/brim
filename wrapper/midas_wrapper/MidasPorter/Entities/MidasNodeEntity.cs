using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasNodeEntity
    {
        private string _nodeNumber;
        private Vector3 _coord;

        public string NodeNumber { get { return _nodeNumber; } set { _nodeNumber = value; } }
        public Vector3 Coord { get { return _coord; } set { _coord = value; } }
        public double X { get { return _coord.X; } }
        public double Y { get { return _coord.Y; } }
        public double Z { get { return _coord.Z; } }
        public MidasNodeEntity() { _coord = new Vector3(); }
        public MidasNodeEntity(string Number, Vector3 vector)
        {
            _coord.X = vector.X;
            _coord.Y = vector.Y;
            _coord.Z = vector.Z;
            _nodeNumber = Number;
        }
        
        public static Dictionary<string, MidasNodeEntity> ReadStrings(StreamReader sr)
        {
            Dictionary<string, MidasNodeEntity> result = new Dictionary<string, MidasNodeEntity>();
            List<string> strList;
            List<string> allPoints=new List<string>();
            MidasNodeEntity pt;

            string ptID;
            string strLine = sr.ReadLine();
            while (strLine[0] == ';')
            {
                strLine = sr.ReadLine();
            }

            while (strLine!="")
            {
                strList = StringUtility.Split(strLine, ",");
                pt=new MidasNodeEntity();
                ptID=strList[0];
                pt.NodeNumber = ptID;
                pt.Coord.X = Convert.ToDouble(strList[1]);
                pt.Coord.Y = Convert.ToDouble(strList[2]);
                pt.Coord.Z = Convert.ToDouble(strList[3]);
                result.Add(ptID, pt);
                strLine = sr.ReadLine();
            }
            return result;

        }

    }

    public class Vector3{
        public double X;
        public double Y;
        public double Z;
    }
}
