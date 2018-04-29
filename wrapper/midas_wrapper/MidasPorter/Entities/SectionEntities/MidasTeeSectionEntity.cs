using System.Text;
namespace Porter.Midas.Entities.SectionEntities
{
    public class MidasTeeSectionEntity : MidasSectionEntity
    {
        private string _db;
        private string _dbname;
        private double _h;
        private double _b;
        private double _b1;
        private double _b2;
        private double _tw;
        private double _tf;
        private bool _isUnderTee;

        public string DB { get { return _db; } set { _db = value; } }
        public string Dbname { get { return _dbname; } set { _dbname = value; } }
        public double H { get { return _h; } set { _h = value; } }
        public double B { get { return _b; } set { _b = value; } }
        public double b1 { get { return _b1; } set { _b1 = value; } }
        public double b2 { get { return _b2; } set { _b2 = value; } }
        public bool IsUnderT { get { return _isUnderTee; }set { _isUnderTee = value; } }
        public double Tw { get { return _tw; } set { _tw = value; } }
        public double Tf { get { return _tf; } set { _tf = value; } }

        public MidasTeeSectionEntity(MidasSectionEntity ent)
		{
			SecName = ent.SecName;
            Number = ent.Number;
            Shape = ent.Shape;
            DataType = ent.DataType;
		}
    }
}
