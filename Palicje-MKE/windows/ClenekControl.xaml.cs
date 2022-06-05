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

using Palicje_MKE.lib;

namespace Palicje_MKE.windows
{
    /// <summary>
    /// Interaction logic for ClenekControl.xaml
    /// </summary>
    public partial class ClenekControl : UserControl
    {
        private TextBox MessageBox;
        private NastavitveOken winProp;
        private bool podporaChecked
        {
            get;
            set;
        }
        public ClenekControl(NastavitveOken winProp)
        {
            this.winProp = winProp;
            this.DataContext = this;
            podporaChecked = false;

            InitializeComponent();
        }

        private void Koordinata_Changed(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            try
            {
                double res = Convert.ToDouble(tb.Text);
                // assign to point
            }
            catch
            {
                winProp.messageBox.Text = "Napaka! Koordinate morajo biti številčne vrednosti.";
            }
        }
    }
}
