using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Porter.Midas.Entities;
using Porter.Midas.Entities.SectionEntities;
using Utility;

namespace Porter.Midas.Entities
{
    public class MidasLineEntity 
    {
        private string _lineName;
        private double _lineBeta;
        private List<string> _lineNode = new List<string>();
        private MidasMaterialEntity _lineMat = new MidasMaterialEntity();
        private MidasSectionEntity _lineSec = new MidasSectionEntity();
        private string _lineSubType;

        public string LineName { get { return _lineName; } set { _lineName = value; } }
        public double LineBeta { get { return _lineBeta; } set { _lineBeta = value; } }
        public List<string> LineNode { get { return _lineNode; } set { _lineNode = value; } }
        public MidasMaterialEntity LineMat { get { return _lineMat; } set { _lineMat = value; } }
        public MidasSectionEntity LineSec { get { return _lineSec; } set { _lineSec = value; } }
        public string LineSubType { get { return _lineSubType; } set { _lineSubType = value; } }
    }

    public class MidaseLineRelease
    {
        public bool HasComponentValue = false;
        public bool[] ReleaseComponentsAtI=new bool[6];
        public bool[] ReleaseComponentsAtJ=new bool[6];
        public double[] ComponentValuesAtI=new double[6];
        public double[] ComponentValuesAtJ=new double[6];
    }
}
