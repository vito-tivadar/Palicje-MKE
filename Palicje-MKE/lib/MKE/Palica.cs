using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Palicje_MKE.lib.MKE
{
    public class Palica : NadzorujObjekt
    {
        private PipeVisual3D palicaVisual3D = new PipeVisual3D();

        private Clenek _clenek1;
        public Clenek clenek1
        {
            get { return _clenek1; }
            set
            {
                _clenek1 = value;
                OnPropertyChanged();
            }
        }

        private Clenek _clenek2;
        public Clenek clenek2
        {
            get { return _clenek2; }
            set
            { 
                _clenek2 = value;
                OnPropertyChanged();
            }
        }
        private string _ime;

        public string ime
        {
            get { return clenek1.ime + clenek2.ime; }
            set
            {
                _ime = value;
                OnPropertyChanged();
            }
        }


        private double _dolzina;

        public double dolzina
        {
            get { return PreracunajDolzino(); }
        }

        public double ploscinaPrereza;
        public double modulElasticnosti;

        public Palica(Clenek clenek1, Clenek clenek2/*, double ploscinaPrereza, double modulElasticnosti*/)
        {
            this.clenek1 = clenek1;
            this.clenek2 = clenek2;
            /* 
            _dolzina = PreracunajDolzino(clenek 1.koordinate, clenek2.koordinate);                  //dodaj preračun glede na koordinate
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

        public void DodajlPalico3D(PipeVisual3D palica3D)
        {
            palicaVisual3D = palica3D;
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
    }





    public class Palice : ObservableCollection<Palica>
    {
        public Collection<Palica> palice;  // private in pridobi z get()

        public Palice()
        {
            palice = new Collection<Palica>();
        }
        public bool PalicaObstaja(Point3D koordinate1, Point3D koordinate2)
        {
            foreach (Palica p in base.Items)
            {
                if (p.clenek1.koordinate == koordinate1 && p.clenek2.koordinate == koordinate2 ||
                    p.clenek1.koordinate == koordinate2 && p.clenek2.koordinate == koordinate1)
                    return true;
            }
            return false;
        }

        public bool PalicaObstaja(string imeKoordinate1, string imeKoordinate2)
        {
            foreach (Palica p in base.Items)
            {
                if (p.ime == $"{imeKoordinate1}{imeKoordinate2}" ||
                    p.ime == $"{imeKoordinate2}{imeKoordinate1}")
                    return true;
            }
            return false;
        }

        public Palica PridobiPalico(Point3D koordinate1, Point3D koordinate2)
        {
            foreach (Palica p in base.Items)
            {
                if (p.clenek1.koordinate == koordinate1 && p.clenek2.koordinate == koordinate2 ||
                    p.clenek1.koordinate == koordinate2 && p.clenek2.koordinate == koordinate1)
                    return p;
            }
            return null;
        }

        public Palica PridobiPalico(string imeKoordinate1, string imeKoordinate2)
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
            if (PalicaObstaja(p.clenek1.koordinate, p.clenek2.koordinate))
            {
                App.sporocilo.SetError($"Palica {p.ime} že obstaja!");
                return false;
            }
            base.Add(p);
            App.sporocilo.SetText($"Dodana je bila palica {p.ime}");
            return true;
        }

        public void OdstraniPalico(Point3D koordinate1, Point3D koordinate2)
        {
            Palica p = PridobiPalico(koordinate1, koordinate2);
            base.Remove(p);
            /*
             foreach palica.ime that contains clenek.ime = remove
             
             
             */
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
