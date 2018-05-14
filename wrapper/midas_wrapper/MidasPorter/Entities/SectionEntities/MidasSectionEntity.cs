using System;
using System.Collections.Generic;
using System.IO;
using Utility;

namespace Porter.Midas.Entities.SectionEntities
{
    public class MidasSectionEntity
    {
        private string _number;
        //private string _secType;
        private string _secName;
        /*private string _offset;
        private string _icent;
        private string _iref;
        private string _ihorz;
        private double _huser;
        private string _ivert;
        private double _vuser;
        private bool _bsd;*/
        private string _shape;
        private string _dataType;
        /*private string _db;
        private string _dbname;
        private List<double> _data = new List<double>();*/

        public string Number { get { return _number; } set { _number = value; } }
        //public string SecType { get { return _secType; } set { _secType = value; } }
        public string SecName { get { return _secName; } set { _secName = value; } }

        public string Offset { get; private set; }="CC";
        public string ICent { get; private set; }="0";
        public string IRef { get; private set; }="0";
        public string IHorizontalZ { get; private set; }="0";
        public double HUser { get; private set; }
        public string IVertical { get; private set; }="0";
        public double VUser { get; private set; }
        public bool ShearDeformation { get; private set; }=true;
        public bool WrinkleEffect { get; private set; }

        /*public string Offset { get { return _offset; } set { _offset = value; } }
public string Icent { get { return _icent; } set { _icent = value; } }
public string Iref { get { return _iref; } set { _iref = value; } }
public string Ihorz { get { return _ihorz; } set { _ihorz = value; } }
public double Huser { get { return _huser; } set { _huser = value; } }
public string Ivert { get { return _ivert; } set { _ivert = value; } }
public double Vuser { get { return _vuser; } set { _vuser = value; } }
public bool Bsd { get { return _bsd; } set { _bsd = value; } }*/
        public string Shape { get { return _shape; } set { _shape = value; } }
        public string DataType { get { return _dataType; } set { _dataType = value; } }
        /*public string DB { get { return _db; } set { _db = value; } }
        public string Dbname { get { return _dbname; } set { _dbname = value; } }
        public List<double> Data { get { return _data; } set { _data = value; } }*/

        public static Dictionary<string, MidasSectionEntity> ReadStrings(StreamReader sr)
        {
            Dictionary<string, MidasSectionEntity> result = new Dictionary<string, MidasSectionEntity>();
            MidasSectionEntity sec;
            List<string> strList;
            string secID;
            string str = sr.ReadLine();
            
            while (str[0] == ';')
            {
                str = sr.ReadLine();
            }

            while (str != "")
            {
                strList = StringUtility.Split(str, ",");
                sec = new MidasSectionEntity();
                secID = strList[0];
                sec.Number = secID;
                //sec.SecType = strList[1];
                sec.SecName = strList[2];
                sec.Offset = strList[3];
                sec.ICent = strList[4];
                sec.IRef = strList[5];
                sec.IHorizontalZ = strList[6];
                sec.HUser = Convert.ToDouble(strList[7]);
                sec.IVertical = strList[8];
                sec.VUser = Convert.ToDouble(strList[9]);
                sec.ShearDeformation = (strList[10] == "YES" ? true : false);
                sec.WrinkleEffect = (strList[11] == "YES" ? true : false);
                sec.Shape = strList[12];
                sec.DataType = strList[13];

                switch (sec.Shape)
                {
                    case "SB":
                        sec = new MidasRectangleSectionEntity(sec);
                        switch (sec.DataType)
                        {
                            case "1":
                                (sec as MidasRectangleSectionEntity).DB = strList[14];
                                (sec as MidasRectangleSectionEntity).Dbname = strList[15];
                                break;
                            case "2":
                                (sec as MidasRectangleSectionEntity).Height = Convert.ToDouble(strList[14]);
                                (sec as MidasRectangleSectionEntity).Width = Convert.ToDouble(strList[15]);
                                break;
                        }
                        break;
                    case "SR":
                        sec = new MidasCircleSectionEntity(sec);
                        switch (sec.DataType)
                        {
                            case "1":
                                (sec as MidasCircleSectionEntity).DB = strList[14];
                                (sec as MidasCircleSectionEntity).Dbname = strList[15];
                                break;
                            case "2":
                                (sec as MidasCircleSectionEntity).Diameter = Convert.ToDouble(strList[14]);
                                break;
                        }
                        break;
                    case "H":
                        sec = new MidasGongSectionEntity(sec);
                        switch (sec.DataType)
                        {
                            case "1":
                                (sec as MidasGongSectionEntity).DB = strList[14];
                                (sec as MidasGongSectionEntity).Dbname = strList[15];
                                break;
                            case "2":
                                (sec as MidasGongSectionEntity).H = Convert.ToDouble(strList[14]);
                                (sec as MidasGongSectionEntity).B1 = Convert.ToDouble(strList[15]);
                                (sec as MidasGongSectionEntity).TW = Convert.ToDouble(strList[16]);
                                (sec as MidasGongSectionEntity).T1 = Convert.ToDouble(strList[17]);
                                if (strList[18] == "0")
                                {
                                    (sec as MidasGongSectionEntity).B2 = Convert.ToDouble(strList[15]);
                                }
                                else
                                {
                                    (sec as MidasGongSectionEntity).B2 = Convert.ToDouble(strList[18]);
                                }
                                if (strList[19] == "0")
                                {
                                    (sec as MidasGongSectionEntity).T2 = Convert.ToDouble(strList[17]);
                                }
                                else
                                {
                                    (sec as MidasGongSectionEntity).T2 = Convert.ToDouble(strList[19]);
                                }
                                break;
                        }
                        break;
                    case "L":
                        sec = new MidasAngleSectionEntity(sec);
                        switch (sec.DataType)
                        {
                            case "1":
                                (sec as MidasAngleSectionEntity).DB = strList[14];
                                (sec as MidasAngleSectionEntity).Dbname = strList[15];
                                break;
                            case "2":
                                (sec as MidasAngleSectionEntity).H = Convert.ToDouble(strList[14]);
                                (sec as MidasAngleSectionEntity).B = Convert.ToDouble(strList[15]);
                                (sec as MidasAngleSectionEntity).Tw = Convert.ToDouble(strList[16]);
                                (sec as MidasAngleSectionEntity).Tf = Convert.ToDouble(strList[17]);
                                break;
                        }
                        break;
                    case "B":
                        sec = new MidasBoxSectionEntity(sec);
                        switch (sec.DataType)
                        {
                            case "1":
                                (sec as MidasBoxSectionEntity).DB = strList[14];
                                (sec as MidasBoxSectionEntity).Dbname = strList[15];
                                break;
                            case "2":
                                (sec as MidasBoxSectionEntity).H = Convert.ToDouble(strList[14]);
                                (sec as MidasBoxSectionEntity).B = Convert.ToDouble(strList[15]);
                                (sec as MidasBoxSectionEntity).Tw = Convert.ToDouble(strList[16]);
                                (sec as MidasBoxSectionEntity).Tf1 = Convert.ToDouble(strList[17]);
                                (sec as MidasBoxSectionEntity).C = Convert.ToDouble(strList[18]);
                                if (strList[19]=="0")
                                {
                                    (sec as MidasBoxSectionEntity).Tf2 = Convert.ToDouble(strList[17]);
                                }
                                else
                                {
                                    (sec as MidasBoxSectionEntity).Tf2 = Convert.ToDouble(strList[19]);
                                }
                                break;
                        }
                        break;
                    case "2L":
                        sec = new MidasDoubleAngleSectionEntity(sec);
                        switch (sec.DataType)
                        {
                            case "1":
                                (sec as MidasDoubleAngleSectionEntity).DB = strList[14];
                                (sec as MidasDoubleAngleSectionEntity).Dbname = strList[15];
                                break;
                            case "2":
                                (sec as MidasDoubleAngleSectionEntity).H = Convert.ToDouble(strList[14]);
                                (sec as MidasDoubleAngleSectionEntity).B = Convert.ToDouble(strList[15]);
                                (sec as MidasDoubleAngleSectionEntity).Tw = Convert.ToDouble(strList[16]);
                                (sec as MidasDoubleAngleSectionEntity).Tf = Convert.ToDouble(strList[17]);
                                (sec as MidasDoubleAngleSectionEntity).C = Convert.ToDouble(strList[18]);
                                break;
                        }
                        break;
                    case "P":
                        sec = new MidasPipeSectionEntity(sec);
                        switch (sec.DataType)
                        {
                            case "1":
                                (sec as MidasPipeSectionEntity).DB = strList[14];
                                (sec as MidasPipeSectionEntity).Dbname = strList[15];
                                break;
                            case "2":
                                (sec as MidasPipeSectionEntity).D = Convert.ToDouble(strList[14]);
                                (sec as MidasPipeSectionEntity).Tw = Convert.ToDouble(strList[15]);
                                break;
                        }
                        break;
                    case "T":
                        sec = new MidasTeeSectionEntity(sec);
                        switch (sec.DataType)
                        {
                            case "1":
                                (sec as MidasTeeSectionEntity).DB = strList[14];
                                (sec as MidasTeeSectionEntity).Dbname = strList[15];
                                break;
                            case "2":
                                (sec as MidasTeeSectionEntity).H = Convert.ToDouble(strList[14]);
                                (sec as MidasTeeSectionEntity).B = Convert.ToDouble(strList[15]);
                                (sec as MidasTeeSectionEntity).Tw = Convert.ToDouble(strList[16]);
                                (sec as MidasTeeSectionEntity).Tf = Convert.ToDouble(strList[17]);
                                break;
                        }
                        break;
                    case "UDT":
                        sec = new MidasTeeSectionEntity(sec);
                        (sec as MidasTeeSectionEntity).IsUnderT = true;
                        switch (sec.DataType)
                        {
                            case "1":
                                (sec as MidasTeeSectionEntity).DB = strList[14];
                                (sec as MidasTeeSectionEntity).Dbname = strList[15];
                                break;
                            case "2":
                                (sec as MidasTeeSectionEntity).H = Convert.ToDouble(strList[14]);
                                (sec as MidasTeeSectionEntity).b1 = Convert.ToDouble(strList[15]);
                                (sec as MidasTeeSectionEntity).b2 = Convert.ToDouble(strList[16]);
                                (sec as MidasTeeSectionEntity).B = (sec as MidasTeeSectionEntity).b1 +
                                                                   (sec as MidasTeeSectionEntity).b2;
                                (sec as MidasTeeSectionEntity).Tw = Convert.ToDouble(strList[17]);
                                (sec as MidasTeeSectionEntity).Tf = Convert.ToDouble(strList[18]);
                                break;
                        }
                        break;
                }
                result.Add(secID, sec);
                str = sr.ReadLine();
            }
            return result;
        }
    }
}
