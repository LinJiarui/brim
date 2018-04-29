using System.Collections.Generic;
using System.IO;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasUnitEntity 
    {
        private string _forceUnit;
        private string _lengthUnit;
        private string _heatUnit;
        private string _temperUnit;

        public string ForceUnit { get { return _forceUnit; } set { _forceUnit = value; } }
        public string LengthUnit { get { return _lengthUnit; } set { _lengthUnit = value; } }
        public string HeatUnit { get { return _heatUnit; } set { _heatUnit = value; } }
        public string TemperUnit { get { return _temperUnit; } set { _temperUnit = value; } }

        public void ReadStrings(StreamReader sr)
        {
            string str = sr.ReadLine();
            while (str[0] == ';')
            {
                str = sr.ReadLine();
            }
            List<string> units = StringUtility.Split(str,",");
            _forceUnit = units[0];
            _lengthUnit = units[1];
            _heatUnit = units[2];
            _temperUnit = units[3];
        }
    }
}
