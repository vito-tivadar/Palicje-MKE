using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Palicje_MKE.lib.MKE
{
    public class Palica
    {
        public Clenek clenek1;
        public Clenek clenek2;

        private string _ime;
        public string ime
        {
            get { return clenek1.ime + clenek2.ime; }
            set { _ime = value; }
        }

        public double dolzina { get { return PreracunajDolzino(); } }

        public double ploscinaPrereza;
        public double modulElasticnosti;

        public Palica(Clenek clenek1, Clenek clenek2/*, double ploscinaPrereza, double modulElasticnosti*/)
        {
            this.clenek1 = clenek1;
            this.clenek2 = clenek2;
            /* 
            _ploscinaPrereza = ploscinaPrereza;             
            _modulElasticnosti = modulElasticnosti;
            */
        }

        private void PosodobiPrviClenek(object sender, EventArgs e)
        {
            clenek2 = sender as Clenek;
        }
        private void PosodobiDrugiClenek(object sender, EventArgs e)
        {
            clenek2 = sender as Clenek;
        }

        public double PreracunajDolzino()
        {
            Point3D c1 = this.clenek1.koordinate;
            Point3D c2 = this.clenek2.koordinate;

            double x = Math.Pow(c2.X - c1.X, 2);
            double y = Math.Pow(c2.Y - c1.Y, 2);
            double z = Math.Pow(c2.Z - c1.Z, 2);

            return Math.Sqrt(x + y + z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="gPS">Število vseh globalnih prostostnih stopenj.</param>
        /// <returns></returns>
        public double[,] TogostnaMatrikaElementa(int gPS)
        {
            double d = dolzina;

            double[] t = new double[] {
                (clenek2.koordinate.X - clenek1.koordinate.X) / d,      // cx
                (clenek2.koordinate.Y - clenek1.koordinate.Y) / d,      // cy
                (clenek2.koordinate.Z - clenek1.koordinate.Z) / d,      // cz
            };

            double[,] k = new double[gPS, gPS];                             // togostna matrika

            int clenek1PS = clenek1.gPS;
            int clenek2PS = clenek2.gPS;

            for (int v = 0; v < 3; v++)
            {
                for (int s = 0; s < 3; s++)
                {
                    double a =((ploscinaPrereza * modulElasticnosti)/d) * t[v] * t[s];

                    k[v, s] = a;
                    k[v + 3, s] = -a;
                    k[v, s + 3] = -a;
                    k[v + 3, s + 3] = a;
                }
            }

            return k;
        }
    }





    public class Palice : Collection<Palica>
    {
        public bool Obstaja(Point3D koordinate1, Point3D koordinate2)
        {
            foreach (Palica p in base.Items)
            {
                if (p.clenek1.koordinate == koordinate1 && p.clenek2.koordinate == koordinate2 ||
                    p.clenek1.koordinate == koordinate2 && p.clenek2.koordinate == koordinate1)
                    return true;
            }
            return false;
        }

        public bool Obstaja(string imeKoordinate1, string imeKoordinate2)
        {
            foreach (Palica p in base.Items)
            {
                if (p.ime == $"{imeKoordinate1}{imeKoordinate2}" ||
                    p.ime == $"{imeKoordinate2}{imeKoordinate1}")
                    return true;
            }
            return false;
        }

        public Palica Pridobi(Point3D koordinate1, Point3D koordinate2)
        {
            foreach (Palica p in base.Items)
            {
                if (p.clenek1.koordinate == koordinate1 && p.clenek2.koordinate == koordinate2 ||
                    p.clenek1.koordinate == koordinate2 && p.clenek2.koordinate == koordinate1)
                    return p;
            }
            return null;
        }

        public Palica Pridobi(string imeKoordinate1, string imeKoordinate2)
        {
            foreach (Palica p in base.Items)
            {
                if (p.ime == $"{imeKoordinate1}{imeKoordinate2}" ||
                    p.ime == $"{imeKoordinate2}{imeKoordinate1}")
                    return p;
            }
            return null;
        }

        public bool Dodaj(Palica p)
        {
            if (Obstaja(p.clenek1.koordinate, p.clenek2.koordinate))
            {
                App.sporocilo.SetError($"Palica {p.ime} že obstaja!");
                return false;
            }
            base.Items.Add(p);
            App.sporocilo.SetText($"Dodana je bila palica {p.ime}");
            return true;
        }

        public void Odstrani(Point3D koordinate1, Point3D koordinate2)
        {
            Palica p = Pridobi(koordinate1, koordinate2);
            if(p != null) base.Items.Remove(p);
        }
        
        public void Odstrani(Palica p)
        {
            if(p != null) base.Items.Remove(p);
        }

        public void Odstrani(Point3D koordinate)
        {
            Collection<Palica> paliceZaIzbris = new Collection<Palica>();

            foreach (Palica palica in base.Items)
            {
                if(palica.clenek1.koordinate == koordinate || palica.clenek2.koordinate == koordinate)
                    paliceZaIzbris.Add(palica);
            }

            foreach (Palica palica in paliceZaIzbris)
            {
                base.Items.Remove(palica);
            }
        }
    }



    public class PalicaNeObstajaException : Exception
    {
        public PalicaNeObstajaException() { }

        public PalicaNeObstajaException(string name) : base(String.Format("Palica ne obstaja: {0}", name))
        {

        }
    }
}
