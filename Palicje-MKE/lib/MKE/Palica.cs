using System;
using System.Collections.ObjectModel;
using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;

namespace Palicje_MKE.lib.MKE
{
    public class Palica : NadzorujObjekt
    {
        private Clenek _clenek1;
        public Clenek clenek1
        {
            get { return _clenek1; }
            set
            {
                _clenek1 = value;
                OnPropertyChanged();
            }
        }

        private Clenek _clenek2;
        public Clenek clenek2
        {
            get { return _clenek2; }
            set
            { 
                _clenek2 = value;
                OnPropertyChanged();
            }
        }

        private int ID { get; }
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
            Random r = new Random();
            ID = new Random().Next(10000, 100000);
        }

        private PipeVisual3D pridobiPalico3D(HelixViewport3D viewPort, PipeVisual3D palica)
        {
            return new PipeVisual3D();
        }

        public void PosodobiPalico3D(Point3D clenek1, Point3D clenek2)
        {

        }
        
        public void PosodobiVidezPalice3D()
        {

        }


    }
}
