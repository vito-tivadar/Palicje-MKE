using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Palicje_MKE.lib
{
    public class NadzorujObjekt : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
