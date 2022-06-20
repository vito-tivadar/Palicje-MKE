using System.Windows;
using System.Windows.Controls;
using Palicje_MKE.lib;
using Palicje_MKE.lib.MKE;

namespace Palicje_MKE.windows
{
    /// <summary>
    /// Interaction logic for PosodobiClenek.xaml
    /// </summary>
    public partial class PosodobiClenek : UserControl
    {
        
        public PosodobiClenek()
        {
            InitializeComponent();
        }

        public void DodajModelKonstrukcije(ModelKonstrukcije konstrukcija)
        {
            clenekControl.konstrukcija = konstrukcija;
        }
        
        private void OdstraniClenek_Click(object sender, RoutedEventArgs e)
        {
            Clenek c = clenekControl.clenek;
            clenekControl.konstrukcija.clenki.Odstrani(c);
            clenekControl.konstrukcija.palice.Odstrani(c.koordinate);
            clenekControl.konstrukcija.OdstraniVisualElement(c.ime);
            this.Visibility = Visibility.Collapsed;
        }

        public void SetClenek(Clenek c)
        {
            clenekControl.SetClenek(c);
        }
    }
}
