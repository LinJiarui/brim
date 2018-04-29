namespace Porter.Midas.Entities.SectionEntities
{
    public class MidasCustomSectionEntity : MidasSectionEntity
    {
        //axis direction
        //           ^ z
        //           |
        //           |
        //           |
        //           ------->y
        public string MidasShape { get; set; } = "GEN";
        public string MidasType { get; set; } = "VALUE";
        public string Offset { get; set; } = "CC";
        public string Blt { get; set; } = "ROLL";
        /// <summary>
        /// consider shear deformation
        /// </summary>
        public bool bSD { get; set; } = true;
        public bool bWE { get; set; } = false;

        public double[] Ds { get; set; } = new double[9];
        public double[] Ys { get; set; } = new double[4];
        public double[] Zs { get; set; } = new double[4];
        public double Area { get; set; }
        public double Asy { get; set; }
        public double Asz { get; set; }
        public double Ixx { get; set; }
        public double Iyy { get; set; }
        public double Izz { get; set; }
        public double Zyy { get; set; }
        public double Zzz { get; set; }
        public double Cyp { get; set; }
        public double Cym { get; set; }
        public double Czp { get; set; }
        public double Czm { get; set; }
        public double Qyb { get; set; }
        public double Qzb { get; set; }
        public double PeriOut { get; set; }
        public double PeriIn { get; set; }
        public double Cy { get; set; }
        public double Cz { get; set; }
        public MidasCustomSectionEntity(MidasSectionEntity ent)
        {
            SecName = ent.SecName;
            Number = ent.Number;
            DataType = ent.DataType;
        }
    }
}