using System;
using System.Collections.Generic;
using System.IO;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasStructypeEntity 
    {
        private string _strucType;
        private string _invertMass;
        private string _howToMass;
        private bool _massOffset;
        private bool _selfWeight;
        private double _gravity;
        private double _temper;
        private bool _alignBeam;
        private bool _alignSlab;

        public string StrucType { get { return _strucType; } set { _strucType = value; } }
        public string InvertMass { get { return _invertMass; } set { _invertMass = value; } }
        public string HowToMass { get { return _howToMass; } set { _howToMass = value; } }
        public bool MassOffset { get { return _massOffset; } set { _massOffset = value; } }
        public bool SelfWeight { get { return _selfWeight; } set { _selfWeight = value; } }
        public double Gravity { get { return _gravity; } set { _gravity = value; } }
        public double Temper { get { return _temper; } set { _temper = value; } }
        public bool AlignBeam { get { return _alignBeam; } set { _alignBeam = value; } }
        public bool AlignSlab { get { return _alignSlab; } set { _alignSlab = value; } }

        public void ReadStrings(StreamReader sr)
        {
            string str = sr.ReadLine();
            while (str[0] == ';')
            {
                str = sr.ReadLine();
            }
            List<string> strList = StringUtility.Split(str,",");
            _strucType = strList[0];
            _invertMass = strList[1];
            _howToMass = strList[2];
            _massOffset = (strList[3] == "YES" ? true : false);
            _selfWeight = (strList[4] == "YES" ? true : false);
            _gravity = Convert.ToDouble(strList[5]);
            _temper = Convert.ToDouble(strList[6]);
            _alignBeam = (strList[7] == "YES" ? true : false);
            _alignSlab = (strList[8] == "YES" ? true : false);
        }
    }
}
