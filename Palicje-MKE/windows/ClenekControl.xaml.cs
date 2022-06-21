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
            prejsnjaPodpora = clenek.podpora;

            PosodobiPolja();
        }

        public void RazveljaviSpremembe()
        {
            clenek.koordinate = prejsnjeKoordinate;
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

            X_checkBox.IsChecked = clenek.podpora.X;
            Y_checkBox.IsChecked = clenek.podpora.Y;
            Z_checkBox.IsChecked = clenek.podpora.Z;
        }

        private void PosodobiClenek(object sender, RoutedEventArgs e)
        {
            if (clenek == null) return;
            Point3D p;
            try
            {
                p = new Point3D(Convert.ToDouble(X.Text), Convert.ToDouble(Y.Text), Convert.ToDouble(Z.Text));
            }
            catch
            {
                RazveljaviSpremembe();
                App.sporocilo.SetError("Koordinate morajo biti številčne vrednosti.");
                return;
            }

            if (konstrukcija == null)
            {
                clenek.koordinate = p;
                prejsnjeKoordinate = p;
                clenek.PosodobiPrejsnjeIme();
                return;
            }
            else 
            {
                if (konstrukcija.clenki.Pridobi(p) != null)
                {
                    App.sporocilo.SetError($"Členek s koordinatami ({p}) že obstaja. :)");
                    RazveljaviSpremembe();
                    return;
                }
                clenek.koordinate = p;
                prejsnjeKoordinate = p;
                konstrukcija.PosodobiVisualClenek(clenek);
                konstrukcija.changed = true;
            }
        }

        private void OdstraniClenek_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            clenek.podpora.X = (bool)X_checkBox.IsChecked;
            clenek.podpora.Y = (bool)Y_checkBox.IsChecked;
            clenek.podpora.Z = (bool)Z_checkBox.IsChecked;
        }
    }
}
