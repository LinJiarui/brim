using System.Text;
namespace Porter.Midas.Entities.SectionEntities
{
    public class MidasCircleSectionEntity : MidasSectionEntity
    {
        private double _diameter;
        private string _db;
        private string _dbname;

        public double Diameter { get { return _diameter; } set { _diameter = value; } }
        public string DB { get { return _db; } set { _db = value; } }
        public string Dbname { get { return _dbname; } set { _dbname = value; } }

        public MidasCircleSectionEntity(MidasSectionEntity ent)
		{
            SecName = ent.SecName;
            Number = ent.Number;
            Shape = ent.Shape;
            DataType = ent.DataType;
		}
    }
}
