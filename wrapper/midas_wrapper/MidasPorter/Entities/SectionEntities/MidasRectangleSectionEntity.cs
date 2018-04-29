using System.Text;
namespace Porter.Midas.Entities.SectionEntities
{
    public class MidasRectangleSectionEntity : MidasSectionEntity
    {
        private double _width;
        private double _height;
        private string _db;
        private string _dbname;

        public double Width { get { return _width; } set { _width = value; } }
        public double Height { get { return _height; } set { _height = value; } }
        public string DB { get { return _db; } set { _db = value; } }
        public string Dbname { get { return _dbname; } set { _dbname = value; } }

        public MidasRectangleSectionEntity(MidasSectionEntity ent)
		{
			SecName = ent.SecName;
            Number = ent.Number;
            Shape = ent.Shape;
            DataType = ent.DataType;
		}
    }
}
