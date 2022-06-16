using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palicje_MKE.lib
{
    public class SporociloAplikacije : NadzorujObjekt
    {
        private string _messageText;

        public string messageText
        {
            get { return _messageText; }
            set
            {
                _messageText = value;
                OnPropertyChanged();
            }
        }

        public class MyProperties
        {
            public static string messageText
            {
                get { return App.messageText; }
            }
        }
    }
}
