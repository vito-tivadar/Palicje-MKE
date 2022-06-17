using HelixToolkit.Wpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using Palicje_MKE.lib.MKE;

namespace Palicje_MKE.lib
{
    public class ModelKonstrukcije : NadzorujObjekt
    {
        public Clenki clenki;
        public Palice palice;

        public ModelVisual3D visualModel;

        public ModelKonstrukcije()
        {
            clenki = new Clenki();
            palice = new Palice();

            visualModel = new ModelVisual3D();
        }






        public void PocistiElemente()
        {
            visualModel = new ModelVisual3D();
        }

        public void UndoLast()
        {
            int count = visualModel.Children.Count;
            if (count > 0)
            {
                visualModel.Children.RemoveAt(visualModel.Children.Count - 1);
                OnPropertyChanged(nameof(visualModel));
            }
        }

        public void DodajClenek(Clenek clenek)
        {
            if (clenki.Dodaj(clenek))
            {
                clenek.ClenekUpdated += PosodobiVisualPalico;

                SphereVisual3D visualClenek = new SphereVisual3D();
                visualClenek.Center = clenek.koordinate;
                visualClenek.Radius = 0.20;
                visualClenek.SetName(clenek.ime);

                visualModel.Children.Add(visualClenek);
                OnPropertyChanged(nameof(visualModel));
            }
        }

        public void DodajPalico(Palica palica)
        {
            if (palice.Dodaj(palica))
            {
                PipeVisual3D visualPalica = new PipeVisual3D();
                visualPalica.Point1 = palica.clenek1.koordinate;
                visualPalica.Point2 = palica.clenek2.koordinate;

                visualPalica.Diameter = 0.14;
                visualPalica.SetName(palica.ime);

                palica.DodajlPalico3D(visualPalica);
                visualModel.Children.Add(visualPalica);
                OnPropertyChanged(nameof(visualModel));
            }
            
        }

        public Clenek GetClenek(string ime)
        {
            foreach (Clenek c in clenki.clenki)
            {
                if(c.ime == ime) return c;
            }
            return null;
        }

        private void PosodobiVisualPalico(object sender, ClenekEventArgs e)
        {
            string ime = e.clenek.prejsnjeIme;
            /*
            foreach (Visual3D item in collection)
            {

            }
            */

        }

        public void FindElementsWithPoint(Point3D koordinate)
        {
            
        }



        public Material NovMaterial(Color color)
        {
            return MaterialHelper.CreateMaterial(color);
        }
    }
}
