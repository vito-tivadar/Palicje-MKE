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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Palicje_MKE.windows
{
    /// <summary>
    /// Interaction logic for PalicaControl.xaml
    /// </summary>
    public partial class PalicaControl : UserControl
    {
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
        }

        public void PrikaziOdstraniPalico()
        {
            OdstraniPalico_button.Visibility = Visibility.Visible;
        }

        private void OdstraniPalico_button_Click(object sender, RoutedEventArgs e)
        {

        }

        public void PosodobiCombobox(string[] imenaClenkov)
        {
            prviClenek.Items.Clear();
            drugiClenek.Items.Clear();

            foreach (string imeClenka in imenaClenkov)
            {
                prviClenek.Items.Add(imeClenka);
                drugiClenek.Items.Add(imeClenka);
            }


        }
    }
}
