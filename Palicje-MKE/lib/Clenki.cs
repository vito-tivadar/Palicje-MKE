using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;

namespace Palicje_MKE.lib
{
    public class Clenki
    {
        public Collection<Clenek> clenki;

        public Clenki()
        {
            clenki = new Collection<Clenek>();
        }
        
        
        public void DodajClenek(double x, double y, double z)
        {
            Point3D p = new Point3D(x, y, z);
            if(clenki.Any(v => v.koordinate == p))
            {
                int id = clenki.Count + 1;
                clenki.Add(new Clenek(p, id));
            }
            else
            {
                //message that exists
            }
        }

        public void OdstraniVozlisce(int id)
        {
            try
            {
                Clenek cl = clenki.First(c => c.ID == id);
                if (cl != null)
                {
                    clenki.Remove(cl);
                }
            }
            catch
            {
                //message that point cant be deleted
            }
        }
        
        public void OdstraniClenek(double x, double y, double z)
        {
            try
            {
                Point3D p = new Point3D(x, y, z);
                Clenek cl = clenki.First(c => c.koordinate == p);
                if (cl != null)
                {
                    clenki.Remove(cl);
                }
            }
            catch
            {
                //message that point cant be deleted
            }
        }

        private void PosodobiIdClenkov()
        {
            for (int i = 0; i < clenki.Count; i++)
            {
                clenki[i].ID = i + 1;
            }
        }

        

    }

    
    public class Clenek
    {

        public Point3D koordinate { get; set; }
        public int ID { get; set; }

        public Clenek(Point3D point, int id)
        {
            this.koordinate = point;
            this.ID = id;
        }

    }
}
