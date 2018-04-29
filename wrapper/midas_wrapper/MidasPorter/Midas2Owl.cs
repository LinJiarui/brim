using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Porter.Midas.Entities.SectionEntities;

namespace Porter.Midas
{
    public class Midas2Owl
    {
        protected Dictionary<string, string> default_xmlns = new Dictionary<string, string>();
        protected Dictionary<string, string> imported_xmlns = new Dictionary<string, string>();
        protected string base_uri;
        protected MidasPorterData porter_data;
        protected Dictionary<string, string> unit_mapper = new Dictionary<string, string>();
        protected Dictionary<string, string> struct_type_mapper = new Dictionary<string, string>();
        protected Dictionary<string, string> mass_conversion_mapper = new Dictionary<string, string>();
        public Midas2Owl(string base_xmlns, MidasPorterData data)
        {
            this.base_uri = base_xmlns;
            this.porter_data = data;
            default_xmlns["rdf"] = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
            default_xmlns["owl"] = "http://www.w3.org/2002/07/owl#";
            default_xmlns["xml"] = "http://www.w3.org/XML/1998/namespace";
            default_xmlns["xsd"] = "http://www.w3.org/2001/XMLSchema#";
            default_xmlns["rdfs"] = "http://www.w3.org/2000/01/rdf-schema#";

            unit_mapper["kgf"] = "kgf";
            unit_mapper["kips"] = "kips";
            unit_mapper["kn"] = "kN";
            unit_mapper["lbf"] = "lbf";
            unit_mapper["n"] = "N";
            unit_mapper["tonf"] = "tonf";
            unit_mapper["btu"] = "Btu";
            unit_mapper["cal"] = "cal";
            unit_mapper["kcal"] = "kcal";
            unit_mapper["j"] = "J";
            unit_mapper["kj"] = "kJ";
            unit_mapper["cm"] = "cm";
            unit_mapper["ft"] = "ft";
            unit_mapper["in"] = "in";
            unit_mapper["m"] = "m";
            unit_mapper["mm"] = "mm";
            unit_mapper["c"] = "C";
            unit_mapper["f"] = "F";

            struct_type_mapper["0"] = "3DStructure";
            struct_type_mapper["3"] = "XYPlaneStructure";
            struct_type_mapper["1"] = "XZPlaneStructure";
            struct_type_mapper["2"] = "YZPlaneStructure";
            struct_type_mapper["4"] = "NoZRotationStructure";

            mass_conversion_mapper["0"] = "None";
            mass_conversion_mapper["1"] = "XYZDirection";
            mass_conversion_mapper["2"] = "XYDirection";
            mass_conversion_mapper["3"] = "ZDirection";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prefix">brim</param>
        /// <param name="uri">http://eil.stanford.edu/ontologies/brim#</param>
        public void ImportOntology(string prefix, string uri)
        {
            imported_xmlns[prefix] = uri;
        }

        public void Export(string path)
        {
            using (var writer = new StreamWriter(File.Create(path), Encoding.UTF8))
            {
                ExportHeader(writer);
                ExportIndividuals(writer);
                ExportEnd(writer);
            }
        }

        private void ExportHeader(StreamWriter writer)
        {
            writer.WriteLine("<?xml version=\"1.0\"?>");
            List<string> lines = new List<string>();
            lines.Add(string.Format("<rdf:RDF xmlns=\"{0}\"", base_uri));
            lines.Add(string.Format(" xml:base=\"{0}\"", base_uri.TrimEnd(new char[] { '#', ' ' })));
            foreach (var kv in default_xmlns)
            {
                lines.Add(string.Format(" xmlns:{0}=\"{1}\"", kv.Key, kv.Value));
            }
            foreach (var kv in imported_xmlns)
            {
                lines.Add(string.Format(" xmlns:{0}=\"{1}\"", kv.Key, kv.Value));
            }

            for (int i = 0; i < lines.Count - 1; i++)
            {
                writer.WriteLine(lines[i]);
            }
            writer.WriteLine(lines.Last() + ">");

            writer.WriteLine(string.Format("<owl:Ontology rdf:about=\"{0}\">", base_uri));
            foreach (var kv in imported_xmlns)
            {
                writer.WriteLine(string.Format("<owl:imports rdf:resource=\"{0}\"/>", kv.Value.TrimEnd(new char[] { '#', ' ' })));
            }
            writer.WriteLine(string.Format("</owl:Ontology>"));
        }

        private void ExportIndividuals(StreamWriter writer)
        {
            ExportConfiguration(writer);
            ExportMaterials(writer);
            ExportSections(writer);
            ExportNodes(writer);
            ExportFrameElements(writer);
            ExportPlanarElements(writer);
        }

        private void ExportConfiguration(StreamWriter writer)
        {
            writer.WriteLine(FormatIndividualStart("Configuration_0"));
            writer.WriteLine(FormatIndividualType("brim", "Configuration"));

            writer.WriteLine(FormatPrimitiveProperty("brim", "hasGravityAccleration", porter_data.StructypeEntity.Gravity));
            writer.WriteLine(FormatPrimitiveProperty("brim", "isAlignBeam", porter_data.StructypeEntity.AlignBeam));
            writer.WriteLine(FormatPrimitiveProperty("brim", "isAlignSlab", porter_data.StructypeEntity.AlignSlab));
            if (!string.IsNullOrWhiteSpace(porter_data.StructypeEntity.HowToMass) && porter_data.StructypeEntity.HowToMass != "0")
            {
                writer.WriteLine(FormatIndividualProperty("brim", "hasMassConversionMethod", "_" + mass_conversion_mapper[porter_data.StructypeEntity.HowToMass.ToLower()]));
            }
            if (!string.IsNullOrWhiteSpace(porter_data.StructypeEntity.StrucType))
            {
                writer.WriteLine(FormatIndividualProperty("brim", "hasStructureType", "_" + struct_type_mapper[porter_data.StructypeEntity.StrucType.ToLower()]));
            }

            if (!string.IsNullOrWhiteSpace(porter_data.UnitEntity.ForceUnit))
            {
                writer.WriteLine(FormatIndividualProperty("brim", "hasForceUnit", "_" + unit_mapper[porter_data.UnitEntity.ForceUnit.ToLower()]));
            }
            if (!string.IsNullOrWhiteSpace(porter_data.UnitEntity.HeatUnit))
            {
                writer.WriteLine(FormatIndividualProperty("brim", "hasHeatUnit", "_" + unit_mapper[porter_data.UnitEntity.HeatUnit.ToLower()]));
            }
            if (!string.IsNullOrWhiteSpace(porter_data.UnitEntity.LengthUnit))
            {
                writer.WriteLine(FormatIndividualProperty("brim", "hasLengthUnit", "_" + unit_mapper[porter_data.UnitEntity.LengthUnit.ToLower()]));
            }
            if (!string.IsNullOrWhiteSpace(porter_data.UnitEntity.TemperUnit))
            {
                writer.WriteLine(FormatIndividualProperty("brim", "hasTemperatureUnit", "_" + unit_mapper[porter_data.UnitEntity.TemperUnit.ToLower()]));
            }


            writer.WriteLine(FormatIndividualEnd());

            //put out individuals for enumration data
            ExportEnumIndividual(writer, "brim", "kgf");
            ExportEnumIndividual(writer, "brim", "kips");
            ExportEnumIndividual(writer, "brim", "kN");
            ExportEnumIndividual(writer, "brim", "lbf");
            ExportEnumIndividual(writer, "brim", "N");
            ExportEnumIndividual(writer, "brim", "tonf");
            ExportEnumIndividual(writer, "brim", "Btu");
            ExportEnumIndividual(writer, "brim", "cal");
            ExportEnumIndividual(writer, "brim", "J");
            ExportEnumIndividual(writer, "brim", "kcal");
            ExportEnumIndividual(writer, "brim", "kJ");
            ExportEnumIndividual(writer, "brim", "cm");
            ExportEnumIndividual(writer, "brim", "ft");
            ExportEnumIndividual(writer, "brim", "in");
            ExportEnumIndividual(writer, "brim", "m");
            ExportEnumIndividual(writer, "brim", "mm");
            ExportEnumIndividual(writer, "brim", "F");
            ExportEnumIndividual(writer, "brim", "C");

            ExportEnumIndividual(writer, "brim", "3DStructure");
            ExportEnumIndividual(writer, "brim", "XYPlaneStructure");
            ExportEnumIndividual(writer, "brim", "XZPlaneStructure");
            ExportEnumIndividual(writer, "brim", "YZPlaneStructure");
            ExportEnumIndividual(writer, "brim", "NoZRotationStructure");

            ExportEnumIndividual(writer, "brim", "XYDirection");
            ExportEnumIndividual(writer, "brim", "XYZDirection");
            ExportEnumIndividual(writer, "brim", "ZDirection");
        }

        private void ExportMaterials(StreamWriter writer)
        {
            foreach (var kv in porter_data.MatDict)
            {
                var key = kv.Key;
                var mat = kv.Value;

                writer.WriteLine(FormatIndividualStart("Material_" + mat.MatNumber));
                var matType = "Material";

                switch (mat.MatType)
                {
                    case "STEEL":
                        matType = "Steel";
                        break;
                    case "CONC":
                        matType = "Concrete";
                        break;
                    case "SRC":
                        break;
                }

                writer.WriteLine(FormatIndividualType("brim", matType));
                writer.WriteLine(FormatPrimitiveProperty("brim", "hasName", mat.MatName));
                writer.WriteLine(FormatPrimitiveProperty("brim", "hasThermalConductivityCoefficient", mat.HeatCo));
                writer.WriteLine(FormatPrimitiveProperty("brim", "hasSpecificHeat", mat.Spheat));
                writer.WriteLine(FormatPrimitiveProperty("brim", "hasUnitWeight", mat.Den));
                if (mat.BMass)
                {
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasUnitMass", mat.Mass));
                }
                if (mat.DataType == "3")
                {
                    writer.WriteLine(FormatIndividualProperty("brim", "hasMaterialType", "_Orthoropic"));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasElasticModulusX", mat.Es[0]));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasElasticModulusY", mat.Es[1]));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasElasticModulusZ", mat.Es[2]));

                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasShearModulusXY", mat.Ss[0]));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasShearModulusXZ", mat.Ss[1]));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasShearModulusYZ", mat.Ss[2]));

                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasPoissonRatioXY", mat.Ps[0]));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasPoissonRatioXZ", mat.Ps[1]));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasPoissonRatioYZ", mat.Ps[2]));

                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasThermalExpansionCoefficientX", mat.Ts[0]));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasThermalExpansionCoefficientY", mat.Ts[1]));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasThermalExpansionCoefficientZ", mat.Ts[2]));

                }
                else
                {
                    writer.WriteLine(FormatIndividualProperty("brim", "hasMaterialType", "_Isotropic"));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasElasticModulus", mat.Elast));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasPoissonRatio", mat.Poisson));
                    writer.WriteLine(FormatPrimitiveProperty("brim", "hasThermalExpansionCoefficient", mat.Thermal));
                }

                //TODO deal with Plast and TUnit properties
                writer.WriteLine(FormatIndividualEnd());
            }

            //put out individuals for enumration data
            ExportEnumIndividual(writer, "brim", "Orthoropic");
            ExportEnumIndividual(writer, "brim", "Isotropic");
            ExportEnumIndividual(writer, "brim", "Uniaxial");
        }

        private void ExportEnumIndividual(StreamWriter writer, string prefix, string type)
        {
            writer.WriteLine(FormatIndividualStart("_" + type));
            writer.WriteLine(FormatIndividualType(prefix, type));
            writer.WriteLine(FormatIndividualEnd());
        }

        private void ExportSections(StreamWriter writer)
        {
            //TODO
            foreach (var kv in porter_data.SecDict)
            {
                var key = kv.Key;
                var section = kv.Value;
                writer.WriteLine(FormatIndividualStart("LineSection_" + section.Number));
                var secType = "LineSection";
                switch (section.Shape)
                {
                    case "T":
                        secType = "LineSectionTShape";
                        writer.WriteLine(FormatIndividualType("brim", secType));
                        var tee = section as MidasTeeSectionEntity;
                        if (tee != null)
                        {
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasHeight", tee.H));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasWebThickness", tee.Tw));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasFlangeThickness", tee.Tf));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasFlangeWidth", tee.B));
                        }
                        break;
                    case "C":
                        secType = "LineSectionCShape";
                        writer.WriteLine(FormatIndividualType("brim", secType));
                        var channel = section as MidasChannelSectionEntity;
                        if (channel != null)
                        {
                            //TODO maybe cold formed channel is the right one
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasHeight", channel.H));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasWebThickness", channel.Tw));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasTopFlangeThickness", channel.Tw));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasBottomFlangeThickness", channel.Tw));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasTopFlangeWidth", channel.B));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasBottomFlangeWidth", channel.B));
                        }
                        break;
                    case "I":
                    case "H":
                        secType = "LineSectionIShape";
                        writer.WriteLine(FormatIndividualType("brim", secType));
                        var gong = section as MidasGongSectionEntity;
                        if (gong != null)
                        {
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasHeight", gong.H));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasWebThickness", gong.TW));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasTopFlangeThickness", gong.T1));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasBottomFlangeThickness", gong.T2));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasTopFlangeWidth", gong.B1));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasBottomFlangeWidth", gong.B2));
                        }
                        break;
                    case "L":
                        secType = "LineSectionAngle";
                        writer.WriteLine(FormatIndividualType("brim", secType));
                        var angle = section as MidasAngleSectionEntity;
                        if (angle != null)
                        {
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasHeight", angle.H));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasWebThickness", angle.Tw));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasFlangeThickness", angle.Tf));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasFlangeWidth", angle.B));
                        }
                        break;
                    case "SB":
                        secType = "LineSectionRectangle";
                        writer.WriteLine(FormatIndividualType("brim", secType));
                        var rect = section as MidasRectangleSectionEntity;
                        if (rect != null)
                        {
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasWidth", rect.Width));
                            writer.WriteLine(FormatPrimitiveProperty("brim", "hasHeight", rect.Height));
                        }
                        break;
                }
                writer.WriteLine(FormatPrimitiveProperty("brim", "hasName", section.SecName));


                //todo section offset and alignment
                writer.WriteLine(FormatIndividualEnd());
            }

            foreach (var kv in porter_data.ThickDict)
            {
                var key = kv.Key;
                var thick = kv.Value;

                writer.WriteLine(FormatIndividualStart("AreaSection_" + thick.ThickId));
                writer.WriteLine(FormatIndividualType("brim", "Shell"));
                writer.WriteLine(FormatPrimitiveProperty("brim", "hasBendThickness", thick.ThickOut));
                writer.WriteLine(FormatPrimitiveProperty("brim", "hasMembraneThickness", thick.ThickIn));
                writer.WriteLine(FormatIndividualProperty("brim", "hasShellSectionType", "_ShellThin"));
                writer.WriteLine(FormatIndividualEnd());
            }

            //put out individuals for enumration data
            ExportEnumIndividual(writer, "brim", "ShellThin");
            ExportEnumIndividual(writer, "brim", "PlateThin");
            ExportEnumIndividual(writer, "brim", "ShellThick");
            ExportEnumIndividual(writer, "brim", "PlateThick");
            ExportEnumIndividual(writer, "brim", "Membrane");
        }

        private void ExportNodes(StreamWriter writer)
        {
            List<string> constraints = new List<string>();

            foreach (var kv in porter_data.NodeDict)
            {
                var key = kv.Key;
                var node = kv.Value;
                writer.WriteLine(FormatIndividualStart("Node_" + node.NodeNumber));
                writer.WriteLine(FormatIndividualType("brim", "Node"));
                writer.WriteLine(FormatIndividualProperty("brim", "hasPoint", "Point_" + node.NodeNumber));

                var id = int.Parse(node.NodeNumber);
                if (porter_data.SupportDict.ContainsKey(id))
                {
                    var constraint = porter_data.SupportDict[id];
                    if (constraints.Contains(constraint))
                    {
                        writer.WriteLine(FormatIndividualProperty("brim", "hasRestraintData", "NodeRestraint_" + constraints.IndexOf(constraint)));
                    }
                    else
                    {
                        writer.WriteLine(FormatIndividualProperty("brim", "hasRestraintData", "NodeRestraint_" + constraints.Count));
                        constraints.Add(constraint);
                    }
                }

                writer.WriteLine(FormatIndividualEnd());

                writer.WriteLine(FormatIndividualStart("Point_" + node.NodeNumber));
                writer.WriteLine(FormatIndividualType("brim", "CartesianPoint"));
                writer.WriteLine(FormatPrimitiveProperty("brim", "hasX", node.X));
                writer.WriteLine(FormatPrimitiveProperty("brim", "hasY", node.Y));
                writer.WriteLine(FormatPrimitiveProperty("brim", "hasZ", node.Z));
                writer.WriteLine(FormatIndividualEnd());


                //export constraints or node restraints
                int count = 0;
                foreach (var constraint in constraints)
                {
                    writer.WriteLine(FormatIndividualStart("NodeRestraint_" + count++));
                    writer.WriteLine(FormatIndividualType("brim", "NodeRestraint"));
                    if (constraint[0] == '1') { writer.WriteLine(FormatPrimitiveProperty("brim", "hasRestraintUx", 0)); }
                    if (constraint[1] == '1') { writer.WriteLine(FormatPrimitiveProperty("brim", "hasRestraintUy", 0)); }
                    if (constraint[2] == '1') { writer.WriteLine(FormatPrimitiveProperty("brim", "hasRestraintUz", 0)); }

                    if (constraint[3] == '1') { writer.WriteLine(FormatPrimitiveProperty("brim", "hasRestraintRx", 0)); }
                    if (constraint[4] == '1') { writer.WriteLine(FormatPrimitiveProperty("brim", "hasRestraintRy", 0)); }
                    if (constraint[5] == '1') { writer.WriteLine(FormatPrimitiveProperty("brim", "hasRestraintRz", 0)); }
                    writer.WriteLine(FormatIndividualEnd());
                }
            }
        }

        private void ExportFrameElements(StreamWriter writer)
        {
            foreach (var kv in porter_data.LineDict)
            {
                var key = kv.Key;
                var line = kv.Value;

                writer.WriteLine(FormatIndividualStart("Frame_" + key));
                var lineType = "FrameElement";
                writer.WriteLine(FormatIndividualType("brim", lineType));
                writer.WriteLine(FormatIndividualProperty("brim", "hasNode1", "Node_" + line.LineNode[0]));
                writer.WriteLine(FormatIndividualProperty("brim", "hasNode2", "Node_" + line.LineNode[1]));
                writer.WriteLine(FormatIndividualProperty("brim", "hasMaterial", "Material_" + line.LineMat.MatNumber));
                writer.WriteLine(FormatIndividualProperty("brim", "hasLineSection", "LineSection_" + line.LineSec.Number));

                if (porter_data.FrameReleaseDict.ContainsKey(key))
                {
                    var release = porter_data.FrameReleaseDict[key];
                    if (release.ReleaseComponentsAtI.Any(b => b))
                    {
                        writer.WriteLine(FormatIndividualProperty("brim", "hasFrameReleaseAtNode1", "FrameRelease_" + key + "_1"));
                    }
                    if (release.ReleaseComponentsAtJ.Any(b => b))
                    {
                        writer.WriteLine(FormatIndividualProperty("brim", "hasFrameReleaseAtNode2", "FrameRelease_" + key + "_2"));
                    }
                }

                writer.WriteLine(FormatIndividualEnd());

                if (porter_data.FrameReleaseDict.ContainsKey(key))
                {
                    var release = porter_data.FrameReleaseDict[key];
                    if (release.ReleaseComponentsAtI.Any(b => b))
                    {
                        writer.WriteLine(FormatIndividualStart("FrameRelease_" + key + "_1"));
                        writer.WriteLine(FormatIndividualType("brim", "FrameRelease"));
                        if (release.ReleaseComponentsAtI[0]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseUx", release.HasComponentValue?release.ComponentValuesAtI[0]:0)); }
                        if (release.ReleaseComponentsAtI[1]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseUy", release.HasComponentValue?release.ComponentValuesAtI[1]:0)); }
                        if (release.ReleaseComponentsAtI[2]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseUz", release.HasComponentValue?release.ComponentValuesAtI[2]:0)); }
                        if (release.ReleaseComponentsAtI[3]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseRx", release.HasComponentValue?release.ComponentValuesAtI[3]:0)); }
                        if (release.ReleaseComponentsAtI[4]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseRy", release.HasComponentValue?release.ComponentValuesAtI[4]:0)); }
                        if (release.ReleaseComponentsAtI[5]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseRz", release.HasComponentValue?release.ComponentValuesAtI[5]:0)); }
                        writer.WriteLine(FormatIndividualEnd());
                    }
                    if (release.ReleaseComponentsAtJ.Any(b => b))
                    {
                        writer.WriteLine(FormatIndividualStart("FrameRelease_" + key + "_2"));
                        writer.WriteLine(FormatIndividualType("brim", "FrameRelease"));
                        if (release.ReleaseComponentsAtJ[0]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseUx", release.HasComponentValue ? release.ComponentValuesAtJ[0] : 0)); }
                        if (release.ReleaseComponentsAtJ[1]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseUy", release.HasComponentValue ? release.ComponentValuesAtJ[1] : 0)); }
                        if (release.ReleaseComponentsAtJ[2]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseUz", release.HasComponentValue ? release.ComponentValuesAtJ[2] : 0)); }
                        if (release.ReleaseComponentsAtJ[3]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseRx", release.HasComponentValue ? release.ComponentValuesAtJ[3] : 0)); }
                        if (release.ReleaseComponentsAtJ[4]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseRy", release.HasComponentValue ? release.ComponentValuesAtJ[4] : 0)); }
                        if (release.ReleaseComponentsAtJ[5]) { writer.WriteLine(FormatPrimitiveProperty("brim", "hasFrameReleaseRz", release.HasComponentValue ? release.ComponentValuesAtJ[5] : 0)); }
                        writer.WriteLine(FormatIndividualEnd());
                    }
                }
            }
        }

        private void ExportPlanarElements(StreamWriter writer)
        {
            foreach (var kv in porter_data.AreaDict)
            {
                var key = kv.Key;
                var area = kv.Value;

                writer.WriteLine(FormatIndividualStart("Planar_" + key));
                var type = "PlanarElement";
                writer.WriteLine(FormatIndividualType("brim", type));
                writer.WriteLine(FormatIndividualProperty("brim", "hasAreaNode1", "Node_" + area.AreaNode[0]));
                writer.WriteLine(FormatIndividualProperty("brim", "hasAreaNode2", "Node_" + area.AreaNode[1]));
                writer.WriteLine(FormatIndividualProperty("brim", "hasAreaNode3", "Node_" + area.AreaNode[2]));
                if (area.AreaNode.Count > 3 && int.Parse(area.AreaNode[3]) != 0)
                {
                    writer.WriteLine(FormatIndividualProperty("brim", "hasAreaNode4", "Node_" + area.AreaNode[3]));
                }
                writer.WriteLine(FormatIndividualProperty("brim", "hasMaterial", "Material_" + area.AreaMat.MatNumber));
                writer.WriteLine(FormatIndividualProperty("brim", "hasAreaSection", "AreaSection_" + area.AreaThick.ThickId));
                //TODO add release later
                writer.WriteLine(FormatIndividualEnd());

            }
        }

        private string FormatIndividualStart(string name)
        {
            return string.Format("<owl:NamedIndividual rdf:about=\"{0}#{1}\">", base_uri.TrimEnd(new char[] { '#', ' ' }), name);
        }
        private string FormatIndividualType(string prefix, string type)
        {
            return string.Format("<rdf:type rdf:resource=\"{0}#{1}\"/>", imported_xmlns[prefix].TrimEnd(new char[] { '#', ' ' }), type);
        }

        private string FormatIndividualProperty(string prefix, string propName, string prop_iri)
        {
            return string.Format("<{0}:{1} rdf:resource=\"{2}#{3}\"/>", prefix, propName, base_uri.TrimEnd(new char[] { '#', ' ' }), prop_iri);
        }
        private string FormatPrimitiveProperty(string prefix, string propName, object propValue)
        {
            string propType = "string";
            string xmlns = "http://www.w3.org/2001/XMLSchema";
            var type = propValue.GetType();
            if (type.IsValueType)
            {
                //TODO handling more types
                if (type == typeof(int)) { propType = "int"; }
                else if (type == typeof(float)) { propType = "double"; }
                if (type == typeof(double)) { propType = "double"; }
                if (type == typeof(bool)) { propType = "boolean"; }
            }

            return string.Format("<{0}:{1} rdf:datatype=\"{2}#{3}\">{4}</{0}:{1}>", prefix, propName, xmlns, propType, propValue);
        }

        private string FormatIndividualEnd()
        {
            return string.Format("</owl:NamedIndividual>");
        }

        private void ExportEnd(StreamWriter writer)
        {
            writer.WriteLine("</rdf:RDF>");
        }
    }
}
