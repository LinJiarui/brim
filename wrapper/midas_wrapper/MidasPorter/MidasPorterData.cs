using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Porter.Midas.Entities;
using Porter.Midas.Entities.SectionEntities;
using Utility;

namespace Porter.Midas
{
    public class MidasPorterData 
    {
        private MidasVersionEntity _versionEntity = new MidasVersionEntity();
        private MidasUnitEntity _unitEntity = new MidasUnitEntity();
        private MidasStructypeEntity _structypeEntity = new MidasStructypeEntity();
        private Dictionary<string, MidasNodeEntity> _nodeDict = new Dictionary<string, MidasNodeEntity>();
        private Dictionary<string, MidasGridEntity> _gridDict = new Dictionary<string, MidasGridEntity>();
        private Dictionary<string, MidasAreaEntity> _areaDict = new Dictionary<string, MidasAreaEntity>();
        private Dictionary<string, MidasLineEntity> _lineDict = new Dictionary<string, MidasLineEntity>();
        private Dictionary<string, MidasElementEntity> _elemDict = new Dictionary<string, MidasElementEntity>();
        private Dictionary<string, MidasMaterialEntity> _matDict = new Dictionary<string, MidasMaterialEntity>();
        private Dictionary<string, MidasSectionEntity> _secDict = new Dictionary<string, MidasSectionEntity>();
        private Dictionary<string, MidasThicknessEntity> _thickDict = new Dictionary<string, MidasThicknessEntity>();
        private Dictionary<string, MidasStldcaseEntity> _stldcaseDict = new Dictionary<string, MidasStldcaseEntity>();
        private Dictionary<string, MidasStoryEntity> _storyDict = new Dictionary<string, MidasStoryEntity>();
        private Dictionary<int, string> _supportDict=new Dictionary<int, string>();
        private Dictionary<string, MidaseLineRelease> _frameReleaseDict=new Dictionary<string, MidaseLineRelease>();

        public MidasVersionEntity VersionEntity { get { return _versionEntity; } set { _versionEntity = value; } }
        public MidasUnitEntity UnitEntity { get { return _unitEntity; } set { _unitEntity = value; } }
        public MidasStructypeEntity StructypeEntity { get { return _structypeEntity; } set { _structypeEntity = value; } }
        public Dictionary<string, MidasNodeEntity> NodeDict { get { return _nodeDict; } set { _nodeDict = value; } }
        public Dictionary<string, MidasGridEntity> GridDict { get { return _gridDict; } set { _gridDict = value; } }
        public Dictionary<string, MidasLineEntity> LineDict { get { return _lineDict; } set { _lineDict = value; } }
        public Dictionary<string, MidasAreaEntity> AreaDict { get { return _areaDict; } set { _areaDict = value; } }
        public Dictionary<string, MidasElementEntity> ElemDict { get { return _elemDict; } set { _elemDict = value; } }
        public Dictionary<string, MidasMaterialEntity> MatDict { get { return _matDict; } set { _matDict = value; } }
        public Dictionary<string, MidasSectionEntity> SecDict { get { return _secDict; } set { _secDict = value; } }
        public Dictionary<string, MidasThicknessEntity> ThickDict { get { return _thickDict; } set { _thickDict = value; } }
        public Dictionary<string, MidasStldcaseEntity> StldcaseDict { get { return _stldcaseDict; } set { _stldcaseDict = value; } }
        public Dictionary<string, MidasStoryEntity> StoryDict { get { return _storyDict; } set { _storyDict = value; } }

        public Dictionary<int, string> SupportDict{get { return _supportDict; }set { _supportDict = value; }}

        public Dictionary<string, MidaseLineRelease> FrameReleaseDict{get { return _frameReleaseDict; }set { _frameReleaseDict = value; }}

        public List<MidasSectionPartEntity> Parts { get; set; }=new List<MidasSectionPartEntity>();

        public Dictionary<string,MidasLineEntity> AssignLine()
        {
            Dictionary<string, MidasLineEntity> result = new Dictionary<string, MidasLineEntity>();
            MidasElementEntity elem;
            MidasLineEntity line;
            MidasSectionEntity sec;
            MidasMaterialEntity mat;
            string elemID;
            foreach (string key in _elemDict.Keys)
            {
                elemID = key;
                _elemDict.TryGetValue(elemID, out elem);
                if (elem.ElemType == "BEAM")
                {
                    line = new MidasLineEntity();
                    line.LineName = elem.ElemName;
                    line.LineBeta = elem.ElemBeta;
                    line.LineNode = elem.ElemNode;
                    line.LineSubType = elem.ElemSubType;
                    _secDict.TryGetValue(elem.ElemPro, out sec);
                    _matDict.TryGetValue(elem.ElemMatID, out mat);
                    line.LineMat = mat;
                    line.LineSec = sec;
                    result.Add(key, line);
                }
            }
            return result;
        }
        public Dictionary<string,MidasAreaEntity> AssignArea()
        {
            Dictionary<string, MidasAreaEntity> result = new Dictionary<string, MidasAreaEntity>();
            MidasElementEntity elem;
            MidasAreaEntity area;
            MidasThicknessEntity thick;
            MidasMaterialEntity mat;
            string elemID;
            foreach (string key in _elemDict.Keys)
            {
                elemID = key;
                _elemDict.TryGetValue(key, out elem);
                if (elem.ElemType == "WALL" || elem.ElemType == "PLATE")
                {
                    area = new MidasAreaEntity();
                    area.AreaName = elem.ElemName;
                    area.AreaNode = elem.ElemNode;
                    area.AreaSubType = elem.ElemSubType;
                    _thickDict.TryGetValue(elem.ElemPro, out thick);
                    _matDict.TryGetValue(elem.ElemMatID, out mat);
                    area.AreaThick = thick;
                    area.AreaMat = mat;
                    result.Add(key, area);
                    area.AreaType = elem.ElemType;
                }
            }
            return result;
        }
    }
}
