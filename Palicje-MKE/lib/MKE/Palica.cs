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
        public string ime { get; set; }

        public double dolzina;
        public double ploscinaPrereza;
        public double modulElasticnosti;

        public Palica(Clenek clenek1, Clenek clenek2/*, double ploscinaPrereza, double modulElasticnosti*/)
        {
            this.clenek1 = clenek1;
            this.clenek2 = clenek2;
            PosodobiIme();

            clenek1.PropertyChanged += PosodobiPrviClenek;
            clenek2.PropertyChanged += PosodobiDrugiClenek;
            /* 
            _dolzina = PreracunajDolzino(clenek 1.koordinate, clenek2.koordinate);                             //dodaj preračun glede na koordinate
            _ploscinaPrereza = ploscinaPrereza;             
            _modulElasticnosti = modulElasticnosti;
            */
        }

        private void PosodobiIme()
        {
            this.ime = $"{clenek1.ime}{clenek2.ime}";
        }

        private void PosodobiPrviClenek(object sender, EventArgs e)
        {
            clenek2 = sender as Clenek;
            PosodobiIme();
        }
        private void PosodobiDrugiClenek(object sender, EventArgs e)
        {
            clenek2 = sender as Clenek;
            PosodobiIme();
        }

        public void DodajlPalico3D(PipeVisual3D palica3D)
        {
            palicaVisual3D = palica3D;
        }
    }





    public class Palice
    {
        public Collection<Palica> palice;  // private in pridobi z get()

        public Palice()
        {
            palice = new Collection<Palica>();
        }
        public bool PalicaObstaja(Point3D koordinate1, Point3D koordinate2)
        {
            foreach (Palica p in palice)
            {
                if (p.clenek1.koordinate == koordinate1 && p.clenek2.koordinate == koordinate2 ||
                    p.clenek1.koordinate == koordinate2 && p.clenek2.koordinate == koordinate1)
                    return true;
            }
            return false;
        }

        public bool PalicaObstaja(string imeKoordinate1, string imeKoordinate2)
        {
            foreach (Palica p in palice)
            {
                if (p.ime == $"{imeKoordinate1}{imeKoordinate2}" ||
                    p.ime == $"{imeKoordinate2}{imeKoordinate1}")
                    return true;
            }
            return false;
        }

        public Palica PridobiPalico(Point3D koordinate1, Point3D koordinate2)
        {
            foreach (Palica p in palice)
            {
                if (p.clenek1.koordinate == koordinate1 && p.clenek2.koordinate == koordinate2 ||
                    p.clenek1.koordinate == koordinate2 && p.clenek2.koordinate == koordinate1)
                    return p;
            }
            return null;
        }

        public Palica PridobiPalico(string imeKoordinate1, string imeKoordinate2)
        {
            foreach (Palica p in palice)
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
            palice.Add(p);
            App.sporocilo.SetText($"Dodana je bila palica {p.ime}");
            return true;
        }

        public void OdstraniPalico(Point3D koordinate1, Point3D koordinate2)
        {
            Palica p = PridobiPalico(koordinate1, koordinate2);
            palice.Remove(p);
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
