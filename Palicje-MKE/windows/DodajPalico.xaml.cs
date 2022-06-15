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
using System.Windows.Shapes;
using Palicje_MKE.lib.MKE;

namespace Palicje_MKE.windows
{
    /// <summary>
    /// Interaction logic for DodajPalico.xaml
    /// </summary>
    public partial class DodajPalico : Window
    {
        public string clenek1;
        public string clenek2;

        public DodajPalico()
        {
            InitializeComponent();
        }

        public void DodajSeznamImen(string[] imenaPalic)
        {
            palicaControl.imenaDodanihPalic = imenaPalic;
        }

        private void OK_button_Click(object sender, RoutedEventArgs e)
        {
            clenek1 = palicaControl.prviClenek.Text;
            clenek2 = palicaControl.drugiClenek.Text;
            if (clenek1 == "" || clenek2 == "" || clenek1 == clenek2) this.DialogResult = false;
            else this.DialogResult = true;
            this.Close();
        }
    }
}
