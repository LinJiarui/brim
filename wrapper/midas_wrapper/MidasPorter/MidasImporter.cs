using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Porter.Midas.Entities;
using Porter.Midas.Entities.SectionEntities;
using Utility;

namespace Porter.Midas
{
    public class MidasImporter
    {
        private MidasPorterData _midasData = new MidasPorterData();
        public MidasPorterData MidasData { get { return _midasData; } set { _midasData = value; } }
        public MidasPorterData Import(string fileName)
        {
            if (fileName == "")
            {
                return null;
            }
            StreamReader m_streamReader = new StreamReader(fileName, Encoding.Default);
            string strLine = m_streamReader.ReadLine();

            while (strLine != null)
            {
                if (strLine == "" || strLine[0] == ';')
                {
                    strLine = m_streamReader.ReadLine();
                    continue;
                }

                if (strLine.Contains("*VERSION"))
                {
                    _midasData.VersionEntity.ReadStrings(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }

                if (strLine.Contains("*UNIT"))
                {
                    _midasData.UnitEntity.ReadStrings(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }
                if (strLine.Contains("*STRUCTYPE"))
                {
                    _midasData.StructypeEntity.ReadStrings(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }
                if (strLine.Contains("*GRIDLINE"))
                {
                    _midasData.GridDict = MidasGridEntity.ReadStrings(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }
                if (strLine.Contains("*NODE"))
                {
                    _midasData.NodeDict = MidasNodeEntity.ReadStrings(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }
                if (strLine.Contains("*ELEMENT"))
                {

                    _midasData.ElemDict = MidasElementEntity.ReadStrings(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }
                if (strLine.Contains("*MATERIAL"))
                {
                    _midasData.MatDict = MidasMaterialEntity.ReadStrings(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }
                if (strLine.Contains("*SECTION"))
                {
                    _midasData.SecDict = MidasSectionEntity.ReadStrings(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }
                if (strLine.Contains("THICKNESS"))
                {
                    _midasData.ThickDict = MidasThicknessEntity.ReadStrings(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }
                if (strLine.Contains("*STLDCASE"))
                {
                    _midasData.StldcaseDict = MidasStldcaseEntity.ReadStrings(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }
                if (strLine.Contains("*STORY"))
                {
                    _midasData.StoryDict = MidasStoryEntity.ReadStrings(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }

                //TODO add the following data section for exporter later
                if (strLine.Contains("*CONSTRAINT"))//node support
                {
                    _midasData.SupportDict = ReadSupports(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }
                if (strLine.Contains("*FRAME-RLS"))//beam end release
                {
                    _midasData.FrameReleaseDict = ReadFrameRelease(m_streamReader);
                    strLine = m_streamReader.ReadLine();
                }


                strLine = m_streamReader.ReadLine();
            }
            _midasData.LineDict = _midasData.AssignLine();
            _midasData.AreaDict = _midasData.AssignArea();
            m_streamReader.Dispose();
            return _midasData;
        }

        private Dictionary<int, string> ReadSupports(StreamReader sr)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            string str = sr.ReadLine();
            while (str[0] == ';')
            {
                str = sr.ReadLine();
            }
            while (str != "")
            {
                var strList = StringUtility.Split(str, ",");
                var constraints = strList[1];
                var nodes = strList[0].Split(' ');
                foreach (var node in nodes)
                {
                    int id = 0;
                    if (int.TryParse(node, out id))
                    {
                        result[id] = constraints;
                    }
                    else if (node.Contains("to"))
                    {
                        var toks = node.Split(new string[] { "to" }, StringSplitOptions.None);
                        for (int i = int.Parse(toks[0]), max = int.Parse(toks[1]); i <= max; i++)
                        {
                            result[i] = constraints;
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
                str = sr.ReadLine();
            }

            return result;
        }

        private Dictionary<string, MidaseLineRelease> ReadFrameRelease(StreamReader sr)
        {
            Dictionary<string, MidaseLineRelease> result = new Dictionary<string, MidaseLineRelease>();

            string str = sr.ReadLine();
            while (str[0] == ';')
            {
                str = sr.ReadLine();
            }
            while (str != "")
            {
                if (!str.Trim().EndsWith(","))
                {
                    str += "," + sr.ReadLine();
                }
                var strList = StringUtility.Split(str, ",");
                var release = new MidaseLineRelease();
                release.HasComponentValue = strList[1] == "YES";
                for (int i = 0; i < 6; i++)
                {
                    release.ReleaseComponentsAtI[i] = strList[2][i] == '1';
                }
                for (int i = 0; i < 6; i++)
                {
                    release.ComponentValuesAtI[i] = double.Parse(strList[3 + i]);
                }
                for (int i = 0; i < 6; i++)
                {
                    release.ReleaseComponentsAtJ[i] = strList[9][i] == '1';
                }
                for (int i = 0; i < 6; i++)
                {
                    release.ComponentValuesAtJ[i] = double.Parse(strList[10 + i]);
                }

                result[strList[0]] = release;
                str = sr.ReadLine();
            }

            return result;
        }
    }
}