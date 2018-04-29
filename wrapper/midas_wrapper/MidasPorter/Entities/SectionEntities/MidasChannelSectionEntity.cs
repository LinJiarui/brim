namespace Porter.Midas.Entities.SectionEntities
{
    public class MidasChannelSectionEntity: MidasSectionEntity
    {
        public double H { get; set; }
        public double B { get; set; }
        public double Tw { get; set; }
        public double r { get; set; }
        public double d { get; set; }
        public MidasChannelSectionEntity(MidasSectionEntity ent)
        {
            SecName = ent.SecName;
            Number = ent.Number;
            Shape = ent.Shape;
            DataType = ent.DataType;
        }
    }
}