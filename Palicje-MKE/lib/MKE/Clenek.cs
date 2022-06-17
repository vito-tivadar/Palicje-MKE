using System;
using System.Windows.Media.Media3D;
using System.Collections.ObjectModel;

namespace Palicje_MKE.lib.MKE
{
    public class Clenek : NadzorujObjekt
    {

        private Point3D _koordinate;

        public Point3D koordinate
        {
            get { return _koordinate; }
            set
            {
                _koordinate = value;
                OnPropertyChanged();
            }
        }

        public Podpora podpora { get; set; }

        public string prejsnjeIme { get; set; }
        public string ime { get; set; }


        public Clenek(Point3D koordinate, Podpora podpora)
        {
            this._koordinate = koordinate;
            this.podpora = podpora;
            PosodobiIme();
        }

        private void PosodobiIme()
        {
            this.prejsnjeIme = this.ime;
            this.ime = $"({this.koordinate.X},{this.koordinate.Y},{this.koordinate.Z})";
        }

        public void PosodobiX(double x)
        {
            _koordinate.X = x;
            PosodobiIme();
        }
        public void PosodobiY(double y)
        {
            _koordinate.Y = y;
            PosodobiIme();
        }
        public void PosodobiZ(double z)
        {
            _koordinate.Z = z;
            PosodobiIme();
        }

        public event EventHandler<ClenekEventArgs> ClenekUpdated;

        protected virtual void OnClenekUpdated(Clenek clenek)
        {
            if(ClenekUpdated != null) ClenekUpdated(this, new ClenekEventArgs(clenek));
        }
    }


    public class ClenekEventArgs : EventArgs
    {
        public Clenek clenek { get; set; }

        public ClenekEventArgs(Clenek clenek)
        {
            this.clenek = clenek;
        }
    }



    public class Clenki
    {
        public Collection<Clenek> clenki;       // private in pridobi z get()

        // <ht:BillboardTextVisual3D Position = "5,0,0" Text="5,0,0" DepthOffset="0.01"/>

        public Clenki()
        {
            clenki = new Collection<Clenek>();
        }

        public bool ClenekObstaja(Point3D koordinate)
        {
            foreach (Clenek c in clenki)
            {
                if (c.koordinate == koordinate) return true;
            }
            return false;
        }

        public bool ClenekObstaja(string ime)
        {
            foreach (Clenek c in clenki)
            {
                if (c.ime == ime) return true;
            }
            return false;
        }

        public Clenek PridobiClenek(Point3D koordinate)
        {
            foreach (Clenek c in clenki)
            {
                if (c.koordinate == koordinate) return c;
            }
            return null;
        }

        public string[] PridobiImena()
        {

            string[] imena = new string[clenki.Count];
            for (int i = 0; i < clenki.Count; i++)
            {
                imena[i] = clenki[i].ime;
            }
            return imena;
        }

        public bool Dodaj(Clenek c)
        {
            if (ClenekObstaja(c.koordinate))
            {
                App.sporocilo.SetError($"Členek s koordinatami {c.ime} že obstaja!");
                return false;
            }
            clenki.Add(c);
            App.sporocilo.SetText($"Dodan je bil členek s koordinatami {c.ime}");
            return true;
        }

        public void OdstraniClenek(Point3D koordinate)
        {
            Clenek c = PridobiClenek(koordinate);
            clenki.Remove(c);
        }


    }



    public class ClenekException : Exception
    {
        public ClenekException() {}

        public ClenekException(string name) : base(String.Format("Napaka s členkom: {0}", name)) { }
    }

    public class Podpora
    {
        public bool X { get; set; }
        public bool Y { get; set; }
        public bool Z { get; set; }

        public Podpora(bool nepomična_x = false, bool nepomična_y = false, bool nepomična_z = false)
        {
            this.X = nepomična_x;
            this.Y = nepomična_y;
            this.Z = nepomična_z;
        }
    }
}
