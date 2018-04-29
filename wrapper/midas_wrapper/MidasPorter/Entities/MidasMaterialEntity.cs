using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasMaterialEntity //未处理组合材料
    {
        private string _matNumber;
        private string _matType;
        private string _matName;
        private double _spheat;
        private double _heatCo;
        private string _plast;//不知道这是什么
        private string _tUnit;
        private bool _bMass;
        private string _dataType;
        private string _db;
        private string _dbName;
        private double _elast;
        private double _poisson;
        private double _thermal;
        private double _den;
        private double _mass;
        private List<double> _es = new List<double>();//弹性模量
        private List<double> _ts = new List<double>();//线膨胀系数
        private List<double> _ss = new List<double>();//剪切模量
        private List<double> _ps = new List<double>();//泊松比

        public string MatNumber { get { return _matNumber; } set { _matNumber = value; } }
        public string MatType { get { return _matType; } set { _matType = value; } }
        public string MatName { get { return _matName; } set { _matName = value; } }
        public double Spheat { get { return _spheat; } set { _spheat = value; } }
        public double HeatCo { get { return _heatCo; } set { _heatCo = value; } }
        public string Plast { get { return _plast; } set { _plast = value; } }
        public string TUnit { get { return _tUnit; } set { _tUnit = value; } }
        public bool BMass { get { return _bMass; } set { _bMass = value; } }
        public string DataType { get { return _dataType; } set { _dataType = value; } }
        public string DB { get { return _db; } set { _db = value; } }
        public string DBName { get { return _dbName; } set { _dbName = value; } }
        public double Elast { get { return _elast; } set { _elast = value; } }
        public double Poisson { get { return _poisson; } set { _poisson = value; } }
        public double Thermal { get { return _thermal; } set { _thermal = value; } }
        public double Den { get { return _den; } set { _den = value; } }
        public double Mass { get { return _mass; } set { _mass = value; } }
        public List<double> Es { get { return _es; } set { _es = value; } }
        public List<double> Ts { get { return _ts; } set { _ts = value; } }
        public List<double> Ss { get { return _ss; } set { _ss = value; } }
        public List<double> Ps { get { return _ps; } set { _ps = value; } }

        public static Dictionary<string, MidasMaterialEntity> ReadStrings(StreamReader sr)
        {
            Dictionary<string, MidasMaterialEntity> result = new Dictionary<string, MidasMaterialEntity>();
            MidasMaterialEntity mat;
            List<string> strList;
            string matID;
            string str = sr.ReadLine();
            int i;

            while (str[0] == ';')
            {
                str = sr.ReadLine();
            }
            while (str != "")
            {
                strList = StringUtility.Split(str, ",");
                mat=new MidasMaterialEntity();
                matID = strList[0];
                mat.MatNumber = matID;
                mat.MatType = strList[1];
                mat.MatName = strList[2];
                mat.Spheat = Convert.ToDouble(strList[3]);
                mat.HeatCo = Convert.ToDouble(strList[4]);
                mat.Plast = strList[5];
                mat.TUnit = strList[6];
                mat.BMass = (strList[7] == "YES" ? true : false);
                mat.DataType = strList[8];
                switch (mat.DataType)
                {
                    case "1":
                        mat.DB = strList[9];
                        mat.DBName = strList[10];
                        break;
                    case "2":
                        mat.Elast = Convert.ToDouble(strList[9]);
                        mat.Poisson = Convert.ToDouble(strList[10]);
                        mat.Thermal = Convert.ToDouble(strList[11]);
                        mat.Den = Convert.ToDouble(strList[12]);
                        mat.Mass = Convert.ToDouble(strList[13]);
                        for (i = 1; i <= 3; i++) { mat.Es.Add(mat.Elast); }
                        for (i = 1; i <= 3; i++) { mat.Ts.Add(mat.Thermal); }
                        for (i = 1; i <= 3; i++) { mat.Ps.Add(mat.Poisson); }
                        for (i = 1; i <= 3; i++) { mat.Ss.Add(mat.Elast/2/(1+mat.Poisson)); }
                        break;
                    case "3":
                        for (i = 1; i <= 3; i++) { mat.Es.Add(Convert.ToDouble(strList[8 + i])); }
                        for (i = 1; i <= 3; i++) { mat.Ts.Add(Convert.ToDouble(strList[11 + i])); }
                        for (i = 1; i <= 3; i++) { mat.Ss.Add(Convert.ToDouble(strList[14 + i])); }
                        for (i = 1; i <= 3; i++) { mat.Ps.Add(Convert.ToDouble(strList[17 + i])); }
                        mat.Mass = Convert.ToDouble(strList[21]);
                        break;
                }
                result.Add(matID, mat);
                str = sr.ReadLine();
            }
            return result;
        }


    }
}
