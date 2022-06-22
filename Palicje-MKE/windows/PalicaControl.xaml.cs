using System.Windows.Controls;
using Palicje_MKE.lib;
using Palicje_MKE.lib.MKE;
using System.Windows.Media.Media3D;
using System;
using System.Windows;

namespace Palicje_MKE.windows
{
    /// <summary>
    /// Interaction logic for PalicaControl.xaml
    /// </summary>
    public partial class PalicaControl : UserControl
    {
        public ModelKonstrukcije konstrukcija;
        public Palica palica;
        public string imePalice;
        private Clenek prejsnjiClenek1;
        private Clenek prejsnjiClenek2;
        public double prejsnjiME;
        public double prejsnjaPloscina;

        private string[] _imenaDodanihPalic;
        public string[] imenaDodanihPalic
        {
            get { return _imenaDodanihPalic; }
            set
            {
                _imenaDodanihPalic = value;
                PosodobiCombobox(value);
            }
        }

        public PalicaControl()
        {
            InitializeComponent();
            imePalice = "";
        }
        public string GetImePalice()
        {
            imePalice = prviClenek.SelectedItem.ToString() + drugiClenek.SelectedItem.ToString();
            return imePalice;
        }

        public void SetPalica(Palica palica)
        {
            this.palica = palica;
            prejsnjiClenek1 = palica.clenek1;
            prejsnjiClenek2 = palica.clenek2;
            prejsnjaPloscina = palica.ploscinaPrereza;
            prejsnjiME = palica.modulElasticnosti;

            imenaDodanihPalic = konstrukcija.clenki.PridobiImena();

            PosodobiPolja();
        }

        public void RazveljaviSpremembe()
        {
            palica.clenek1 = prejsnjiClenek1;
            palica.clenek2 = prejsnjiClenek2;
            palica.modulElasticnosti = prejsnjiME;
            palica.ploscinaPrereza = prejsnjaPloscina;

            PosodobiPolja();
        }

        private void PosodobiPolja()
        {
            prviClenek.SelectedItem = prejsnjiClenek1.ime;
            drugiClenek.SelectedItem = prejsnjiClenek2.ime;

            modulElasticnosti.Text = prejsnjiME.ToString();
            ploscina.Text = prejsnjaPloscina.ToString();
        }

        private void PosodobiPalico(object sender, SelectionChangedEventArgs e)
        {
            if (prviClenek.SelectedItem == null || drugiClenek.SelectedItem == null) return;
            if (palica == null || konstrukcija == null) return;

            string imeKoordinate1 = prviClenek.SelectedItem.ToString();
            string imeKoordinate2 = drugiClenek.SelectedItem.ToString();

            if(imeKoordinate1 == palica.clenek1.ime && imeKoordinate2 == palica.clenek2.ime) return;

            if (imeKoordinate1 == imeKoordinate2)
            {
                App.sporocilo.SetError($"Palica ne more povezovati iste koordinate.");
                RazveljaviSpremembe();
                return;
            }

            if(konstrukcija.palice.Obstaja(imeKoordinate1, imeKoordinate2))
            {
                App.sporocilo.SetError($"Palica ki povezije členka: {imeKoordinate1} in {imeKoordinate2}, že obstaja.");
                RazveljaviSpremembe();
                return;
            }

            Clenek c = konstrukcija.clenki.Pridobi(imeKoordinate1);
            palica.clenek1 = konstrukcija.clenki.Pridobi(imeKoordinate1);
            palica.clenek2 = konstrukcija.clenki.Pridobi(imeKoordinate2);
            
            ComboBox cb = sender as ComboBox;
            if (cb.Name == "prviClenek") konstrukcija.PosodobiVisualPalico(palica.clenek1, prejsnjiClenek1.ime);
            else konstrukcija.PosodobiVisualPalico(palica.clenek2, prejsnjiClenek2.ime);
            konstrukcija.changed = true;
        }


        private void PosodobiCombobox(string[] imenaClenkov)
        {
            prviClenek.Items.Clear();
            drugiClenek.Items.Clear();

            foreach (string imeClenka in imenaClenkov)
            {
                prviClenek.Items.Add(imeClenka);
                drugiClenek.Items.Add(imeClenka);
            }

            if(palica == null)
            {
                prviClenek.SelectedItem = prviClenek.Items[0];
                drugiClenek.SelectedItem = drugiClenek.Items[1];
            }
        }

        private void TextChanged(object sender, RoutedEventArgs e)
        {
            double me = 0, p = 0;
            try
            {
                me = Convert.ToDouble(modulElasticnosti.Text);
                p = Convert.ToDouble(ploscina.Text);
            }
            catch
            {
                App.sporocilo.SetError("Modul elastičnosti in ploščina morata biti številčni vrednosti.");
                modulElasticnosti.Text = prejsnjiME.ToString();
                ploscina.Text = prejsnjaPloscina.ToString();
                return;
            }

            palica.modulElasticnosti = me;
            palica.ploscinaPrereza = p;
            konstrukcija.changed = true;
        }
    }
}
