using System;
using System.Windows.Media.Media3D;
using System.Collections.ObjectModel;

namespace Palicje_MKE.lib.MKE
{
    public class Clenek
    {

        private Point3D _koordinate;
        public Point3D koordinate
        {
            get { return _koordinate; }
            set
            {
                _koordinate = value;
                PosodobiIme();
            }
        }
        public Vector3D sila { get; set; }
        public Podpora podpora { get; set; }
        public string prejsnjeIme { get; set; }
        public string ime { get; set; }
        public int gPS { get; set; } = 0;                       // oznaka prostostne stopnje v globalnem sistemu


        public Clenek(Point3D koordinate, Podpora podpora)
        {
            this._koordinate = koordinate;
            this.podpora = podpora;
            PosodobiIme();
            this.prejsnjeIme = this.ime;
        }

        private void PosodobiIme()
        {
            this.ime = $"({this.koordinate})";
        }
        
        public void PosodobiPrejsnjeIme()
        {
            this.prejsnjeIme = this.ime;
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

        public double[] LokalnaSilaClenka(int steviloPS)
        {
            double[] k = new double[steviloPS * 3];

            k[3 * gPS - 3] = sila.X;
            k[3 * gPS - 2] = sila.Y;
            k[3 * gPS - 1] = sila.Z;

            return k;
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



    public class Clenki : Collection<Clenek>
    {
        // <ht:BillboardTextVisual3D Position = "5,0,0" Text="5,0,0" DepthOffset="0.01"/>

        public bool ClenekObstaja(Point3D koordinate)
        {
            foreach (Clenek c in base.Items)
            {
                if (c.koordinate == koordinate) return true;
            }
            return false;
        }

        public bool ClenekObstaja(string ime)
        {
            foreach (Clenek c in base.Items)
            {
                if (c.ime == ime) return true;
            }
            return false;
        }

        public Clenek Pridobi(Point3D koordinate)
        {
            foreach (Clenek c in base.Items)
            {
                if (c.koordinate == koordinate) return c;
            }
            return null;
        }
        
        public Clenek Pridobi(string ime)
        {
            foreach (Clenek c in base.Items)
            {
                if (c.ime == ime) return c;
            }
            return null;
        }

        public string[] PridobiImena()
        {

            string[] imena = new string[base.Items.Count];
            for (int i = 0; i < base.Items.Count; i++)
            {
                imena[i] = base.Items[i].ime;
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
            base.Items.Add(c);
            App.sporocilo.SetText($"Dodan je bil členek s koordinatami {c.ime}");
            return true;
        }

        public void DodajPS(Clenek c, int gPS)
        {
            int i = base.Items.IndexOf(c);
            if (i != -1)
            {
                base.Items[i].gPS = gPS;
            }
        }

        public bool Posodobi(Clenek clenek)
        {
            if(ClenekObstaja(clenek.koordinate) == false)
            {
                App.sporocilo.SetError($"Clenek s koordinatami {clenek.ime} že obstaja :(");
                return false;
            }
            int i = 0;
            foreach (Clenek c in base.Items)
            {
                Console.WriteLine("---------------------------");
                Console.WriteLine(base.Items);
                Console.WriteLine(base.Items[i].koordinate);
                Console.WriteLine("---------------------------");
                if(c.ime == clenek.prejsnjeIme)
                {
                    c.koordinate = clenek.koordinate;
                    return true;
                }
                i++;
            }

            return false;

        }

        public void Odstrani(Point3D koordinate)
        {
            Clenek c = Pridobi(koordinate);
            if (c != null) base.Items.Remove(c);
        }
        
        public void Odstrani(Clenek c)
        {
            if (c != null) base.Items.Remove(c);
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
