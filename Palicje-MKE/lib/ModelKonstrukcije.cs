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

        private bool konstrukcijaChanged = false;
        public void PocistiElemente()
        {
            visualModel = new ModelVisual3D();
            clenki.Clear();
            palice.Clear();
        }

        public void UndoLast()
        {
            int count = visualModel.Children.Count;
            if (count > 0)
            {
                visualModel.Children.RemoveAt(visualModel.Children.Count - 1);
                OnPropertyChanged(nameof(visualModel));
                konstrukcijaChanged = true;
            }
        }

        public bool DodajClenek(Clenek clenek)
        {
            if (clenki.Dodaj(clenek))
            {

                SphereVisual3D visualClenek = new SphereVisual3D();
                visualClenek.Center = clenek.koordinate;
                visualClenek.Radius = 0.20;
                visualClenek.SetName(clenek.ime);

                visualModel.Children.Add(visualClenek);
                OnPropertyChanged(nameof(visualModel));
                konstrukcijaChanged = true;
                App.kamera.Prilagodi();
                return true;
            }
            return false;
        }

        public bool DodajPalico(Palica palica)
        {
            if (palice.Dodaj(palica))
            {
                PipeVisual3D visualPalica = new PipeVisual3D();
                visualPalica.Point1 = palica.clenek1.koordinate;
                visualPalica.Point2 = palica.clenek2.koordinate;
                visualPalica.Diameter = 0.14;
                visualPalica.Material = NovMaterial(Colors.Orange);

                visualPalica.SetName(palica.ime);

                visualModel.Children.Add(visualPalica);
                OnPropertyChanged(nameof(visualModel));
                konstrukcijaChanged = true;
                return true;
            }
            return false;
            
        }

        public void PosodobiVisualClenek(Clenek c)
        {
            string prejsnjeIme = c.prejsnjeIme;

            foreach (Visual3D visual in visualModel.Children)
            {
                if(visual.GetName() == prejsnjeIme)
                {
                    SphereVisual3D clenek = visual as SphereVisual3D;
                    clenek.Center = c.koordinate;
                    c.PosodobiPrejsnjeIme();
                    clenek.SetName(c.ime);
                    continue;
                }
                if (visual.GetName().StartsWith(prejsnjeIme))
                {
                    PipeVisual3D palica = visual as PipeVisual3D;
                    palica.Point1 = c.koordinate;
                    c.PosodobiPrejsnjeIme();
                    System.Console.WriteLine(c.prejsnjeIme);
                    palica.SetName($"({palica.Point1})({palica.Point2})");
                    continue;
                }
                if (visual.GetName().EndsWith(prejsnjeIme))
                {
                    PipeVisual3D palica = visual as PipeVisual3D;
                    palica.Point2 = c.koordinate;
                    c.PosodobiPrejsnjeIme();
                    System.Console.WriteLine(c.prejsnjeIme);
                    palica.SetName($"({palica.Point1})({palica.Point2})");
                    continue;
                }
            }
        }

        public void PosodobiVisualPalico(Clenek c, string prejsnjeIme)
        {
            foreach (Visual3D visual in visualModel.Children)
            {
                if (visual.GetType() != typeof(PipeVisual3D)) continue;
                if (visual.GetName().StartsWith(prejsnjeIme))
                {
                    PipeVisual3D palica = visual as PipeVisual3D;
                    palica.Point1 = c.koordinate;
                    c.PosodobiPrejsnjeIme();
                    System.Console.WriteLine(c.prejsnjeIme);
                    palica.SetName($"({palica.Point1})({palica.Point2})");
                    continue;
                }
                if (visual.GetName().EndsWith(prejsnjeIme))
                {
                    PipeVisual3D palica = visual as PipeVisual3D;
                    palica.Point2 = c.koordinate;
                    c.PosodobiPrejsnjeIme();
                    System.Console.WriteLine(c.prejsnjeIme);
                    palica.SetName($"({palica.Point1})({palica.Point2})");
                    continue;
                }
            }
        }

        public Material NovMaterial(Color color)
        {
            return MaterialHelper.CreateMaterial(color);
        }
    }
}
