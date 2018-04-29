using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Porter.Midas.Entities;
using Porter.Midas.Entities.SectionEntities;
using Utility;

namespace Porter.Midas
{
    public class MidasExporter
    {
        private MidasPorterData _midasPorterData = new MidasPorterData();
        private string _filepath;

        public MidasExporter(string file)
        {
            _filepath = file;
        }
        public void Export(MidasPorterData data)
        {
            _midasPorterData = data as MidasPorterData;
            if (_midasPorterData == null)
            {
                return;
            }
            GenerateMidasMGTFile();
        }

        #region using MidasPorterData to generate mgt fie
        private bool GenerateMidasMGTFile()
        {
            string file = _filepath;
                using (FileStream fileStream = new FileStream(file, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(fileStream, Encoding.Default))
                    {
                        writer.WriteLine("*VERSION");
                        writer.WriteLine(" 7.3.0");
                        writer.WriteLine();
                        WriteUnitInfoToMGT(writer);
                        WriteStructtypeInfoToMGT(writer);
                        WriteNodeInfoToMGT(writer);
                        WriteElementInfoToMGT(writer);
                        WriteMaterialInfoToMGT(writer);
                        WriteSectionInfoToMGT(writer);
                        WriteThickInfoToMGT(writer);
                        writer.WriteLine("*ENDDATA");
                        writer.WriteLine();
                    }
                }
            return true;
        }
        private void WriteUnitInfoToMGT(StreamWriter writer)
        {
            writer.WriteLine("*UNIT");
            if (_midasPorterData.UnitEntity.ForceUnit != null)
            {
                writer.Write(_midasPorterData.UnitEntity.ForceUnit + ",");
            }
            else
            {
                writer.Write("KN,");
            }
            if (_midasPorterData.UnitEntity.LengthUnit != null)
            {
                writer.Write(_midasPorterData.UnitEntity.LengthUnit + ",");
            }
            else
            {
                writer.Write("M,");
            }
            if (_midasPorterData.UnitEntity.HeatUnit != null)
            {
                writer.Write(_midasPorterData.UnitEntity.HeatUnit + ",");
            }
            else
            {
                writer.Write("KJ,");
            }
            if (_midasPorterData.UnitEntity.TemperUnit != null)
            {
                writer.Write(_midasPorterData.UnitEntity.TemperUnit);
            }
            else
            {
                writer.Write("C");
            }
            writer.WriteLine();
            writer.WriteLine();
        }
        private void WriteStructtypeInfoToMGT(StreamWriter writer)
        {
            writer.WriteLine("*STRUCTYPE");
            if (_midasPorterData.StructypeEntity.StrucType != null)
            {
                writer.Write(_midasPorterData.StructypeEntity.StrucType + ",");
            }
            else
            {
                writer.Write("0,");
            }
            if (_midasPorterData.StructypeEntity.InvertMass != null)
            {
                writer.Write(_midasPorterData.StructypeEntity.InvertMass + ",");
            }
            else
            {
                writer.Write("0,");
            }
            if (_midasPorterData.StructypeEntity.HowToMass != null)
            {
                writer.Write(_midasPorterData.StructypeEntity.HowToMass + ",");
            }
            else
            {
                writer.Write("1,");
            }
            if (_midasPorterData.StructypeEntity.MassOffset)
            {
                writer.Write("YES,");
            }
            else
            {
                writer.Write("NO,");
            }
            if (_midasPorterData.StructypeEntity.SelfWeight)
            {
                writer.Write("YES,");
            }
            else
            {
                writer.Write("NO,");
            }
            if (_midasPorterData.StructypeEntity.Gravity != 0)
            {
                writer.Write(_midasPorterData.StructypeEntity.Gravity + ",");
            }
            else
            {
                writer.Write("9.806,");
            }
            writer.Write(_midasPorterData.StructypeEntity.Temper.ToString() + ",");
            if (_midasPorterData.StructypeEntity.AlignBeam)
            {
                writer.Write("YES,");
            }
            else
            {
                writer.Write("NO,");
            }
            if (_midasPorterData.StructypeEntity.AlignSlab)
            {
                writer.Write("YES");
            }
            else
            {
                writer.Write("NO");
            }
            writer.WriteLine();
            writer.WriteLine();
        }
        private void WriteMaterialInfoToMGT(StreamWriter writer)
        {
            int i;
            writer.WriteLine("*MATERIAL");
            foreach (MidasMaterialEntity mat in _midasPorterData.MatDict.Values)
            {
                writer.Write(mat.MatNumber + ",");
                writer.Write(mat.MatType + ",");
                writer.Write(mat.MatName + ",");
                writer.Write("0,0, ,C,NO,");
                writer.Write(mat.DataType + ",");
                switch (mat.DataType)
                {
                    case "1":
                        writer.Write("GB(RC)" + mat.MatName);
                        break;
                    case "2":
                        writer.Write(mat.Es[0] + "," + mat.Ps[0] + "," + mat.Ts[0] + ",");
                        writer.Write("0,");
                        writer.Write(mat.Mass);
                        break;
                    case "3":
                        for (i = 0; i <= 2; i++) { writer.Write(mat.Es[i] + ","); }
                        for (i = 0; i <= 2; i++) { writer.Write(mat.Ts[i] + ","); }
                        for (i = 0; i <= 2; i++) { writer.Write(mat.Ss[i] + ","); }
                        for (i = 0; i <= 2; i++) { writer.Write(mat.Ps[i] + ","); }
                        writer.Write(mat.Mass);
                        break;
                }
                writer.WriteLine();
            }
            writer.WriteLine();
        }
        private void WriteNodeInfoToMGT(StreamWriter writer)
        {
            writer.WriteLine("*NODE");

            foreach (MidasNodeEntity node in _midasPorterData.NodeDict.Values)
            {
                writer.WriteLine(node.NodeNumber + "," + node.X + "," + node.Y + "," + node.Z);
            }
            writer.WriteLine();
        }
        private void WriteSectionInfoToMGT(StreamWriter writer)
        {
            writer.WriteLine("*SECTION");
            foreach (MidasSectionEntity item in _midasPorterData.SecDict.Values)
            {
                if (item is MidasRectangleSectionEntity)
                {
                    MidasRectangleSectionEntity section = item as MidasRectangleSectionEntity;
                    writer.Write(section.Number + ",DBUSER,");
                    writer.Write(section.SecName + ",");
                    writer.Write("CC,0,0,0,0,0,0,YES,");
                    writer.Write("SB,");
                    writer.Write("2,");
                    writer.Write(section.Height + "," + section.Width + ",");
                    writer.WriteLine("0,0,0,0,0,0,0,0");
                }
                if (item is MidasAngleSectionEntity)
                {
                    MidasAngleSectionEntity section = item as MidasAngleSectionEntity;
                    writer.Write(section.Number + ",DBUSER,");
                    writer.Write(section.SecName + ",");
                    writer.Write("CC,0,0,0,0,0,0,YES,");
                    writer.Write("L,");
                    writer.Write("2,");
                    writer.Write(section.H + "," + section.B + "," + section.Tw + "," + section.Tf + ",");
                    writer.WriteLine("0,0,0,0,0,0");
                }
                if (item is MidasBoxSectionEntity)
                {
                    MidasBoxSectionEntity section = item as MidasBoxSectionEntity;
                    writer.Write(section.Number + ",DBUSER,");
                    writer.Write(section.SecName + ",");
                    writer.Write("CC,0,0,0,0,0,0,YES,");
                    writer.Write("B,");
                    writer.Write("2,");
                    writer.Write(section.H + "," + section.B + "," + section.Tw + "," + section.Tf1 + "," + section.C + "," + section.Tf2 + ",");
                    writer.WriteLine("0,0,0,0");
                }
                if (item is MidasCircleSectionEntity)
                {
                    MidasCircleSectionEntity section = item as MidasCircleSectionEntity;
                    writer.Write(section.Number + ",DBUSER,");
                    writer.Write(section.SecName + ",");
                    writer.Write("CC,0,0,0,0,0,0,YES,");
                    writer.Write("SR,");
                    writer.Write("2,");
                    writer.Write(section.Diameter + ",");
                    writer.WriteLine("0,0,0,0,0,0,0,0,0");
                }
                if (item is MidasDoubleAngleSectionEntity)
                {
                    MidasDoubleAngleSectionEntity section = item as MidasDoubleAngleSectionEntity;
                    writer.Write(section.Number + ",DBUSER,");
                    writer.Write(section.SecName + ",");
                    writer.Write("CC,0,0,0,0,0,0,YES,");
                    writer.Write("2L,");
                    writer.Write("2,");
                    writer.Write(section.H + "," + section.B + "," + section.Tw + "," + section.Tf + "," + section.C + ",");
                    writer.WriteLine("0,0,0,0,0");
                }
                if (item is MidasGongSectionEntity)
                {
                    MidasGongSectionEntity section = item as MidasGongSectionEntity;
                    writer.Write(section.Number + ",DBUSER,");
                    writer.Write(section.SecName + ",");
                    writer.Write("CC,0,0,0,0,0,0,YES,");
                    writer.Write("H,");
                    writer.Write("2,");
                    writer.Write(section.H + "," + section.B1 + "," + section.TW + "," + section.T1 + "," + section.B2 + "," + section.T2 + ",");
                    writer.WriteLine("0,0,0,0");
                }
                if (item is MidasPipeSectionEntity)
                {
                    MidasPipeSectionEntity section = item as MidasPipeSectionEntity;
                    writer.Write(section.Number + ",DBUSER,");
                    writer.Write(section.SecName + ",");
                    writer.Write("CC,0,0,0,0,0,0,YES,");
                    writer.Write("P,");
                    writer.Write("2,");
                    writer.Write(section.D + "," + section.Tw + ",");
                    writer.WriteLine("0,0,0,0,0,0,0,0");
                }
                if (item is MidasTeeSectionEntity)
                {
                    MidasTeeSectionEntity section = item as MidasTeeSectionEntity;
                    writer.Write(section.Number + ",DBUSER,");
                    writer.Write(section.SecName + ",");
                    writer.Write("CC,0,0,0,0,0,0,YES,");
                    if (section.IsUnderT)
                    {
                        writer.Write("UDT,");
                        writer.Write("2,");
                        writer.Write(section.H + "," + section.b1 + "," +section.b2 + "," + section.Tw + "," + section.Tf + ",");
                        writer.WriteLine("0,0,0,0,0");
                    }
                    else
                    {
                    writer.Write("T,");
                    writer.Write("2,");
                    writer.Write(section.H + "," + section.B + "," + section.Tw + "," + section.Tf + ",");
                    writer.WriteLine("0,0,0,0,0,0");
                    }
                   
                }
                if (item is MidasChannelSectionEntity)
                {
                    MidasChannelSectionEntity section = item as MidasChannelSectionEntity;
                    writer.Write(section.Number + ",DBUSER,");
                    writer.Write(section.SecName + ",");
                    writer.Write("CC,0,0,0,0,0,0,YES,");
                    writer.Write("CC,");
                    writer.Write("2,");
                    writer.Write(section.H + "," + section.B + "," + section.Tw + "," + section.r + "," + section.d + ",");
                    writer.WriteLine("0,0,0,0,0");
                }
            }
            writer.WriteLine();

            writer.WriteLine("*SECT-PSCVALUE");
            foreach (MidasSectionEntity item in _midasPorterData.SecDict.Values)
            {
                if (item is MidasCustomSectionEntity)
                {
                    MidasCustomSectionEntity section = item as MidasCustomSectionEntity;
                    writer.Write("SECT=" + section.Number + "," + section.MidasType + ",");
                    writer.Write(section.SecName + "," + section.Offset + "," + "0,0,0,0,0,0,");
                    writer.Write((section.bSD ? "YES" : "NO") + "," + (section.bWE ? "YES" : "NO") + "," + section.MidasShape + ",YES,YES");
                    UpdateSectionParasByParts(section);
                    writer.WriteLine();

                    writer.WriteLine("    "+section.Area + "," + section.Asy + "," + section.Asz + "," + section.Ixx + "," + section.Iyy + "," + section.Izz);
                    writer.WriteLine("    "+section.Cyp + "," + section.Cym + "," + section.Czp + "," + section.Czm + "," + section.Qyb + "," + section.Qzb + "," + section.PeriOut + "," + section.PeriIn + "," + section.Cy + "," + section.Cz);

                    writer.Write("    ");
                    for (int i = 0; i < 4; i++)
                    {
                        writer.Write(section.Ys[i] + ",");
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        writer.Write(section.Zs[i] + ",");
                    }
                    //writer.Write(section.Zyy + "," + section.Zzz);
                    writer.WriteLine();
                }
            }
            writer.WriteLine();
        }

        private void UpdateSectionParasByParts(MidasCustomSectionEntity section)
        {
            double minx = double.MaxValue;
            double miny = double.MaxValue;
            double maxx = double.MinValue;
            double maxy = double.MinValue;

            foreach (var part in _midasPorterData.Parts.Where(p => p.SectionName == section.SecName))
            {
                double xc = 0, yc = 0, h = 0, w = 0;
                //TODO consider rotation later, usually not necessary
                if (part.ShapeName.Contains("Plate"))
                {
                    xc = double.Parse(part.GeoDef["XCenter"]);
                    yc = double.Parse(part.GeoDef["YCenter"]);
                    h = double.Parse(part.GeoDef["Thickness"]);
                    w = double.Parse(part.GeoDef["Width"]);
                }
                else if (part.ShapeName.Contains("Rectangle"))
                {
                    xc = double.Parse(part.GeoDef["XCenter"]);
                    yc = double.Parse(part.GeoDef["YCenter"]);
                    h = double.Parse(part.GeoDef["Height"]);
                    w = double.Parse(part.GeoDef["Width"]);
                }
                else
                {
                    Trace.WriteLine("possible error with section part.");
                    continue;
                }

                minx = Math.Min(xc - w / 2, minx);
                miny = Math.Min(yc - h / 2, miny);
                maxx = Math.Max(xc + w / 2, maxx);
                maxy = Math.Max(yc + h / 2, maxy);
            }

            section.Cyp = maxx;
            section.Cym = minx;
            section.Czp = maxy;
            section.Czm = miny;
            section.Ds[0] = maxx - minx;
            section.Ds[1] = maxy - miny;

            section.Cy = Math.Max(-minx, maxx);
            section.Cz = Math.Max(-miny, maxy);
        }

        private void WriteElementInfoToMGT(StreamWriter writer)
        {
            writer.WriteLine("*ELEMENT");
            int i;
            int eId = 1;
            int maxLineId = 0;
            foreach (MidasLineEntity line in _midasPorterData.LineDict.Values)
            {
                writer.Write(line.LineName + ",");
//                writer.Write(eId + ",");
                writer.Write("BEAM,");
                if (line.LineMat != null && line.LineSec != null)
                {
                    writer.Write(line.LineMat.MatNumber + ",");
                    writer.Write(line.LineSec.Number + ",");
                    writer.Write(line.LineNode[0] + ",");
                    writer.Write(line.LineNode[1] + ",");
                }
                else
                {
                    writer.Write(",,");
                    writer.Write(line.LineNode[0] + ",");
                    writer.Write(line.LineNode[1] + ",");
                }
                writer.WriteLine(line.LineBeta+";Frame="+ line.LineName);
                eId++;
                maxLineId = Math.Max(maxLineId, int.Parse(line.LineName));
            }

            eId = maxLineId;
            foreach (MidasAreaEntity area in _midasPorterData.AreaDict.Values)
            {
                writer.Write(eId + ",");
                writer.Write(area.AreaType + ",");
                if (area.AreaMat != null)
                {
                    writer.Write(area.AreaMat.MatNumber + ",");
                }
                else
                {
                    writer.Write(",");
                }
                writer.Write(area.AreaThick.ThickId + ",");
                for (i = 0; i <= area.AreaNode.Count() - 1; i++)
                {
                    writer.Write(area.AreaNode[i] + ",");
                }
                if (area.AreaNode.Count == 3)
                {
                    writer.Write("0,1,0");
                }
                else
                {
                    writer.Write("2,0");
                }
                writer.WriteLine(";Plate="+ area.AreaName.Split('_')[0]);
                eId++;
            }
            writer.WriteLine();
        }
        private void WriteThickInfoToMGT(StreamWriter writer)
        {
            writer.WriteLine("*THICKNESS");
            foreach (MidasThicknessEntity thick in _midasPorterData.ThickDict.Values)
            {
                writer.Write(thick.ThickId + ",");
                writer.Write("VALUE,YES,");
                writer.Write(thick.ThickIn + ",");
                writer.WriteLine(thick.ThickOut);
            }
            writer.WriteLine();
        }
        #endregion
    }
}
