using System.Linq;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace Palicje_MKE.lib.MKE
{
    public class Clenek : NadzorujObjekt
    {

        private Point3D _koordinate;

        public Point3D koordinate
        {
            get { return _koordinate; }
            set { _koordinate = value; }
        }

        public int ID { get; set; }

        public Clenek(Point3D point, int id)
        {
            this._koordinate = point;
            this.ID = id;
        }

        private void PosodobiKoordinate()
        {

        }

    }
}
