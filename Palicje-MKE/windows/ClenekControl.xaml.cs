using System;
using System.Windows;
using System.Windows.Controls;

using Palicje_MKE.lib.MKE;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;


namespace Palicje_MKE.windows
{
    /// <summary>
    /// Interaction logic for ClenekControl.xaml
    /// </summary>
    public partial class ClenekControl : UserControl
    {
        public Clenek clenek;
        public SphereVisual3D sphere;
        private Palica[] povezanePalice;

        public ClenekControl()
        {
            InitializeComponent();
            clenek = new Clenek(new Point3D(0,0,0), new Podpora());
        }

        public void PrikaziOdstraniClenek()
        {
            OdstraniClenek_button.Visibility = Visibility.Visible;
            //clenek = null;
        }

        public void SetClenek(Clenek clenek, SphereVisual3D sphere)
        {
            this.clenek = clenek;
            this.sphere = sphere;
            SetKoordinate();

            //when you change checkboxes it triggers on checked event and disables wrong checkboxes
            X_checkBox.IsChecked = clenek.podpora.X;
            Y_checkBox.IsChecked = clenek.podpora.Y;
            Z_checkBox.IsChecked = clenek.podpora.Z;

            if( clenek.podpora.X == false &&
                clenek.podpora.Y == false &&
                clenek.podpora.Z == false)
                Podpora_checkbox.IsChecked = false;
            else Podpora_checkbox.IsChecked = true;
        }
        
        public Clenek GetClenek()
        {
            return clenek;
        }
        
        public void ClearClenek()
        {
            clenek = null;
            sphere = null;
        }
        
        private void SetKoordinate()
        {
            X.Text = clenek.koordinate.X.ToString();
            Y.Text = clenek.koordinate.Y.ToString();
            Z.Text = clenek.koordinate.Z.ToString();
        }

        private void GetKoordinate(object sender, RoutedEventArgs e)
        {
            if (clenek == null) return;
            TextBox tb = sender as TextBox;
            try
            {
                switch (tb.Name)
                {
                    case "X":
                        clenek.PosodobiX(Convert.ToDouble(X.Text));
                        break;
                    case "Y":
                        clenek.PosodobiY(Convert.ToDouble(Y.Text));
                        break;
                    case "Z":
                        clenek.PosodobiZ(Convert.ToDouble(Z.Text));
                        break;
                }
                if(sphere != null) sphere.Center = clenek.koordinate;
            }
            catch
            {
                App.sporocilo.SetError("Koordinate morajo biti številčne vrednosti.");
            }
        }

        private void OdstraniClenek_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            if ((bool)Podpora_checkbox.IsChecked)
            {
                clenek.podpora.X = (bool)X_checkBox.IsChecked;
                clenek.podpora.Y = (bool)Y_checkBox.IsChecked;
                clenek.podpora.Z = (bool)Z_checkBox.IsChecked;

            }
            else
            {
                clenek.podpora.X = false;
                clenek.podpora.Y = false;
                clenek.podpora.Z = false;
            }
        }

        ///DODAJ ZA PODPORO;
    }
}
