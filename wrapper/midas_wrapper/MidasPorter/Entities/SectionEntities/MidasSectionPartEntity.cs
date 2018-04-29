using System.Collections.Generic;

namespace Porter.Midas.Entities.SectionEntities
{
    public class MidasSectionPartEntity
    {
        public string SectionName { get; set; }
        public string ShapeName { get; set; }
        public string ShapeMat { get; set; }
        public Dictionary<string, string> GeoDef { get; set; } = new Dictionary<string, string>();
    }
}