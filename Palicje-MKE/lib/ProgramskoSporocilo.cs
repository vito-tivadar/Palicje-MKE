using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Palicje_MKE.lib
{
    public class ProgramskoSporocilo : NadzorujObjekt
    {
        private TextBlock _tb;

        public string text
        {
            get { return _tb.Text; }
        }

        public void SetTextBlock(TextBlock tb)
        {
            _tb = tb;
        }

        public void SetText(string sporocilo, string tooltip = "")
        {
            _tb.Text = sporocilo;
            _tb.ToolTip = tooltip;
            _tb.Foreground = new SolidColorBrush(Colors.Black);
            OnPropertyChanged();
        }

        public void SetText(string sporocilo, Color color, string tooltip = "")
        {

            _tb.Text = sporocilo;
            _tb.ToolTip = tooltip;
            _tb.Foreground = new SolidColorBrush(color);
            OnPropertyChanged();
        }
        public void SetError(string sporocilo, string tooltip = "")
        {

            _tb.Text = "Napaka: " + sporocilo;
            _tb.ToolTip = tooltip;
            _tb.Foreground = new SolidColorBrush(Colors.Red);
            OnPropertyChanged();
        }

    }
}
