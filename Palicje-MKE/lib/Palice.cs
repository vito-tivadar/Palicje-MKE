using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Palicje_MKE.lib
{
    public class Palice
    {
        Collection<Palica> _palice;

        public Palice()
        {
            _palice = new Collection<Palica>();
        }
    }


    public class Palica
    {
        public Clenek _clenek1;
        public Clenek _clenek2;

        public double _dolzina;
        public double _ploscinaPrereza;
        public double _modulElasticnosti;

        public Palica(Clenek clenek1, Clenek clenek2, double dolzina, double ploscinaPrereza, double modulElasticnosti)
        {
            _clenek1 = clenek1;
            _clenek2 = clenek2;

            _dolzina = dolzina;
            _ploscinaPrereza = ploscinaPrereza;
            _modulElasticnosti = modulElasticnosti;
        }
    }
}
