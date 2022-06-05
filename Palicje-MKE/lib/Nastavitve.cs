using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Palicje_MKE.lib
{
    public class Nastavitve
    {
    }

    public class NastavitveOken
    {
        public TextBlock messageBox { get; }
        public Clenek clenek { get; set; }
        public Palica palica { get; set; }

        public NastavitveOken(TextBlock textBlock)
        {
            this.messageBox = textBlock;
        }

        public void UpdateMessageBox(string message)
        {
            this.messageBox.Text = message;
        }

    }
}
