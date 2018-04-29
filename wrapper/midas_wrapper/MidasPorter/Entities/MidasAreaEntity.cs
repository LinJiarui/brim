using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Porter.Midas.Entities.SectionEntities;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasAreaEntity
    {
        private string _areaName;
        private string _areaType;
        private string _areaMatID;
        private string _areaPro;
        private List<string> _areaNode = new List<string>();
        private string _areaSubType;
        private string _areaWallID;
        private MidasMaterialEntity _areaMat;
        private MidasThicknessEntity _areaThick;

        public string AreaName { get { return _areaName; } set { _areaName = value; } }
        public string AreaType { get { return _areaType; } set { _areaType = value; } }
        public string AreaMatID { get { return _areaMatID; } set { _areaMatID = value; } }
        public string AreaPro { get { return _areaPro; } set { _areaPro = value; } }
        public List<string> AreaNode { get { return _areaNode; } set { _areaNode = value; } }
        public string AreaSubType { get { return _areaSubType; } set { _areaSubType = value; } }
        public string AreaWallID { get { return _areaWallID; } set { _areaWallID = value; } }
        public MidasMaterialEntity AreaMat { get { return _areaMat; } set { _areaMat = value; } }
        public MidasThicknessEntity AreaThick { get { return _areaThick; } set { _areaThick = value; } }

    }
}
