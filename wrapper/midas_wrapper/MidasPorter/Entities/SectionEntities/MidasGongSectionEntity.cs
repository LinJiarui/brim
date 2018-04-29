using System.Text;
namespace Porter.Midas.Entities.SectionEntities
{
    public class MidasGongSectionEntity : MidasSectionEntity
    {
        private double _h;
        private double _b1;
        private double _b2;
        private double _tw;
        private double _t1;
        private double _t2;
        private string _db;
        private string _dbname;

        public double H { get { return _h; } set { _h = value; } }
        public double B1 { get { return _b1; } set { _b1 = value; } }
        public double B2 { get { return _b2; } set { _b2 = value; } }
        public double TW { get { return _tw; } set { _tw = value; } }
        public double T1 { get { return _t1; } set { _t1 = value; } }
        public double T2 { get { return _t2; } set { _t2 = value; } }
        public string DB { get { return _db; } set { _db = value; } }
        public string Dbname { get { return _dbname; } set { _dbname = value; } }

        public MidasGongSectionEntity(MidasSectionEntity ent)
		{
			SecName = ent.SecName;
            Number = ent.Number;
            Shape = ent.Shape;
            DataType = ent.DataType;
		}
    }
}
