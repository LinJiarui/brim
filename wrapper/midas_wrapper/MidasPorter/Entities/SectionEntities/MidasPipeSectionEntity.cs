using System.Text;
namespace Porter.Midas.Entities.SectionEntities
{
    public class MidasPipeSectionEntity : MidasSectionEntity
    {
        private string _db;
        private string _dbname;
        private double _d;
        private double _tw;

        public string DB { get { return _db; } set { _db = value; } }
        public string Dbname { get { return _dbname; } set { _dbname = value; } }
        public double D { get { return _d; } set { _d = value; } }
        public double Tw { get { return _tw; } set { _tw = value; } }

        public MidasPipeSectionEntity(MidasSectionEntity ent)
		{
			SecName = ent.SecName;
            Number = ent.Number;
            Shape = ent.Shape;
            DataType = ent.DataType;
		}
    }
}
