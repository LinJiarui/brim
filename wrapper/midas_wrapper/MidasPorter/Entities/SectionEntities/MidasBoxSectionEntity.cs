using System.Text;

namespace Porter.Midas.Entities.SectionEntities
{
    public class MidasBoxSectionEntity : MidasSectionEntity
    {
        private string _db;
        private string _dbname;
        private double _h;
        private double _b;
        private double _tw;
        private double _tf1;
        private double _c;
        private double _tf2;

        public string DB { get { return _db; } set { _db = value; } }
        public string Dbname { get { return _dbname; } set { _dbname = value; } }
        public double H { get { return _h; } set { _h = value; } }
        public double B { get { return _b; } set { _b = value; } }
        public double Tw { get { return _tw; } set { _tw = value; } }
        public double Tf1 { get { return _tf1; } set { _tf1 = value; } }
        public double C { get { return _c; } set { _c = value; } }
        public double Tf2 { get { return _tf2; } set { _tf2 = value; } }

        public MidasBoxSectionEntity(MidasSectionEntity ent)
		{
			SecName = ent.SecName;
            Number = ent.Number;
            Shape = ent.Shape;
            DataType = ent.DataType;
		}
    }
}
