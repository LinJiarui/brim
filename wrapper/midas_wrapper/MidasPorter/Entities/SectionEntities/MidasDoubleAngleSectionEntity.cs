using System.Text;
namespace Porter.Midas.Entities.SectionEntities
{
    public class MidasDoubleAngleSectionEntity : MidasSectionEntity
    {
        private string _db;
        private string _dbname;
        private double _h;
        private double _b;
        private double _tw;
        private double _tf;
        private double _c;

        public string DB { get { return _db; } set { _db = value; } }
        public string Dbname { get { return _dbname; } set { _dbname = value; } }
        public double H { get { return _h; } set { _h = value; } }
        public double B { get { return _b; } set { _b = value; } }
        public double Tw { get { return _tw; } set { _tw = value; } }
        public double Tf { get { return _tf; } set { _tf = value; } }
        public double C { get { return _c; } set { _c = value; } }

        public MidasDoubleAngleSectionEntity(MidasSectionEntity ent)
		{
			SecName = ent.SecName;
            Number = ent.Number;
            Shape = ent.Shape;
            DataType = ent.DataType;
		}
    }
}
