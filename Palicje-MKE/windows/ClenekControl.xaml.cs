using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using Palicje_MKE.lib;
using Palicje_MKE.lib.MKE;

using Newtonsoft.Json;
using System.IO;


namespace Palicje_MKE.windows
{
    /// <summary>
    /// Interaction logic for ClenekControl.xaml
    /// </summary>
    public partial class ClenekControl : UserControl
    {
        public Clenek clenek;
        public Point3D prejsnjeKoordinate;
        public Vector3D prejsnjaSila;
        public Podpora prejsnjaPodpora;
        public ModelKonstrukcije konstrukcija;

        public ClenekControl()
        {
            InitializeComponent();
            clenek = new Clenek(new Point3D(0,0,0), new Podpora());
        }

        public void SetClenek(Clenek clenek)
        {
            this.clenek = clenek;
            prejsnjeKoordinate = clenek.koordinate;
            prejsnjaSila = clenek.sila;
            prejsnjaPodpora = clenek.podpora;

            PosodobiPolja();
        }

        public void RazveljaviSpremembe()
        {
            clenek.koordinate = prejsnjeKoordinate;
            clenek.sila = prejsnjaSila;
            clenek.podpora = prejsnjaPodpora;

            PosodobiPolja();
        }
        
        public Clenek GetClenek()
        {
            return clenek;
        }

        private void PosodobiPolja()
        {
            X.Text = clenek.koordinate.X.ToString();
            Y.Text = clenek.koordinate.Y.ToString();
            Z.Text = clenek.koordinate.Z.ToString();
            
            fX.Text = clenek.sila.X.ToString();
            fY.Text = clenek.sila.Y.ToString();
            fZ.Text = clenek.sila.Z.ToString();

            X_checkBox.IsChecked = clenek.podpora.X;
            Y_checkBox.IsChecked = clenek.podpora.Y;
            Z_checkBox.IsChecked = clenek.podpora.Z;
        }

        private void PosodobiClenek(object sender, RoutedEventArgs e)
        {
            if (clenek == null) return;
            Point3D p;
            Vector3D f;
            try
            {
                p = new Point3D(Convert.ToDouble(X.Text), Convert.ToDouble(Y.Text), Convert.ToDouble(Z.Text));
                f = new Vector3D(Convert.ToDouble(fX.Text), Convert.ToDouble(fY.Text), Convert.ToDouble(fZ.Text));
            }
            catch
            {
                RazveljaviSpremembe();
                App.sporocilo.SetError("Koordinate in sile morajo biti številčne vrednosti.");
                return;
            }

            if (konstrukcija == null)
            {
                clenek.koordinate = p;
                prejsnjeKoordinate = p;
                clenek.sila = f;
                prejsnjaSila = f;
                clenek.PosodobiPrejsnjeIme();
                return;
            }
            else 
            {
                if (konstrukcija.clenki.Pridobi(p) != null && prejsnjaSila == f)
                {
                    App.sporocilo.SetError($"Členek s koordinatami ({p}) že obstaja.");
                    RazveljaviSpremembe();
                    return;
                }
                clenek.koordinate = p;
                prejsnjeKoordinate = p;
                clenek.sila = f;
                prejsnjaSila = f;
                konstrukcija.PosodobiVisualClenek(clenek);
                konstrukcija.changed = true;
            }
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            clenek.podpora.X = (bool)X_checkBox.IsChecked;
            clenek.podpora.Y = (bool)Y_checkBox.IsChecked;
            clenek.podpora.Z = (bool)Z_checkBox.IsChecked;
        }
    }
}
