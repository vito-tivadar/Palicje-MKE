using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Palicje_MKE.lib;
using Palicje_MKE.lib.MKE;

namespace Palicje_MKE.windows
{
    /// <summary>
    /// Interaction logic for PosodobiPalico.xaml
    /// </summary>
    public partial class PosodobiPalico : UserControl
    {

        public PosodobiPalico()
        {
            InitializeComponent();
        }

        public void DodajModelKonstrukcije(ModelKonstrukcije konstrukcija)
        {
            palicaControl.konstrukcija = konstrukcija;
        }

        private void OdstraniPalico_Click(object sender, RoutedEventArgs e)
        {
            Palica p = palicaControl.palica;
            palicaControl.konstrukcija.palice.Odstrani(p);
            palicaControl.konstrukcija.OdstraniVisualElement(p.ime);
            this.Visibility = Visibility.Collapsed;
        }
        public void SetPalica(Palica p)
        {
            
            palicaControl.SetPalica(p);
        }
    }
}
