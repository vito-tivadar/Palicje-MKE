using System;
using System.Windows;
using System.Windows.Media.Media3D;
using Palicje_MKE.lib.MKE;

namespace Palicje_MKE.windows
{
    /// <summary>
    /// Interaction logic for DodajClenek.xaml
    /// </summary>
    public partial class DodajClenek : Window
    {
        public Clenek clenek;
        public DodajClenek()
        {
            InitializeComponent();
        }

        private void OK_button_Click(object sender, RoutedEventArgs e)
        {
            clenek = clenekControl.GetClenek();
            this.DialogResult = true;
            this.Close();
        }

        //if(!okClicked) App.sporocilo.SetText("Dodajanje členka je bilo preklicano");
    }
}
