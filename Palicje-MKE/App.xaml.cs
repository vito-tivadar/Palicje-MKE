using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Palicje_MKE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Palicje_MKE.lib.Other.ProgramskoSporocilo sporocilo = new Palicje_MKE.lib.Other.ProgramskoSporocilo();
        public static Palicje_MKE.lib.Helix_Toolkit.Kamera kamera = new Palicje_MKE.lib.Helix_Toolkit.Kamera();
    }
}
