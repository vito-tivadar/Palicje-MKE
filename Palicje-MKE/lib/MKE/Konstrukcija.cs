using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;


namespace Palicje_MKE.lib.MKE
{
    public class Konstrukcija
    {
        private List<Clenek> clenki;
        private Collection<Palica> palice;


        public Konstrukcija()
        {
            clenki = new List<Clenek>();
            palice = new Collection<Palica>();
        }


        public void DodajClenek(double x, double y, double z)
        {
            
            Point3D p = new Point3D(x, y, z);
            if (clenki.Any(v => v.koordinate == p))
            {
                int id = clenki.Count + 1;
                clenki.Add(new Clenek(p, id));
            }
            else
            {
                //message that exists
            }
        }

        public void OdstraniClenek(int id)
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
}
