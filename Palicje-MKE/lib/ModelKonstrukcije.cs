using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.IO;

using Newtonsoft.Json;
using HelixToolkit.Wpf;
using Palicje_MKE.lib.MKE;
using Microsoft.Win32;

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

        public bool changed = false;

        public void Nova()
        {
            visualModel.Children.Clear();
            clenki.Clear();
            palice.Clear();
            changed = false;
            App.sporocilo.SetText("Ustvarjena je bila nova konstrukcija.");
        }

        public void Odpri()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Datoteke Konstrukcij(*.kon)|*.kon|All files (*.*)|*.*";
            ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            ofd.RestoreDirectory = true;

            if ((bool)ofd.ShowDialog())
            {
                Console.WriteLine(ofd.FileName);
                string file = File.ReadAllText(ofd.FileName);
                Tuple<Clenki, Palice> k = JsonConvert.DeserializeObject<Tuple<Clenki, Palice>>(file);
                this.clenki = k.Item1;
                this.palice = k.Item2;
                DodajVisualElemente();
                changed = false;
                App.sporocilo.SetText("Naložena konstrukcija: " + ofd.FileName);
            }
        }

        public bool Shrani()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Datoteke Konstrukcij(*.kon)|*.kon|All files (*.*)|*.*";
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            sfd.RestoreDirectory = true;
            
            if ((bool)sfd.ShowDialog())
            {
                Tuple<Clenki, Palice> k = new Tuple<Clenki, Palice>(clenki, palice);
                string contents = JsonConvert.SerializeObject(k, Formatting.Indented);
                File.WriteAllText(sfd.FileName, contents);
                this.changed = false;
                App.sporocilo.SetText("Konstrukcija shranjena.");
                return true;
            }
            else return false;
        }

        public void UndoLast()
        {
            int count = visualModel.Children.Count;
            if (count > 0)
            {
                visualModel.Children.RemoveAt(visualModel.Children.Count - 1);
                OnPropertyChanged(nameof(visualModel));
                changed = true;
            }
        }

        public bool DodajClenek(Clenek clenek)
        {
            if (clenki.Dodaj(clenek))
            {

                SphereVisual3D visualClenek = new SphereVisual3D();
                visualClenek.Center = clenek.koordinate;
                visualClenek.Radius = 0.20;
                visualClenek.Material = NovMaterial("3498db");
                visualClenek.SetName(clenek.ime);

                visualModel.Children.Add(visualClenek);
                OnPropertyChanged(nameof(visualModel));
                changed = true;
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
                visualPalica.Diameter = 0.20;
                visualPalica.Material = Materials.Orange;

                visualPalica.SetName(palica.ime);

                visualModel.Children.Add(visualPalica);
                OnPropertyChanged(nameof(visualModel));
                changed = true;
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
                    palica.SetName($"({palica.Point1})({palica.Point2})");
                    continue;
                }
                if (visual.GetName().EndsWith(prejsnjeIme))
                {
                    PipeVisual3D palica = visual as PipeVisual3D;
                    palica.Point2 = c.koordinate;
                    c.PosodobiPrejsnjeIme();
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
                    palica.SetName($"({palica.Point1})({palica.Point2})");
                    continue;
                }
                if (visual.GetName().EndsWith(prejsnjeIme))
                {
                    PipeVisual3D palica = visual as PipeVisual3D;
                    palica.Point2 = c.koordinate;
                    c.PosodobiPrejsnjeIme();
                    palica.SetName($"({palica.Point1})({palica.Point2})");
                    continue;
                }
            }
        }

        private void DodajVisualElemente()
        {
            visualModel.Children.Clear();
            foreach (Clenek clenek in clenki)
            {
                SphereVisual3D visualClenek = new SphereVisual3D();
                visualClenek.Center = clenek.koordinate;
                visualClenek.Radius = 0.20;
                visualClenek.Material = NovMaterial("3498db");
                visualClenek.SetName(clenek.ime);

                visualModel.Children.Add(visualClenek);
                
                changed = true;
            }

            foreach (Palica palica in palice)
            {
                PipeVisual3D visualPalica = new PipeVisual3D();
                visualPalica.Point1 = palica.clenek1.koordinate;
                visualPalica.Point2 = palica.clenek2.koordinate;
                visualPalica.Diameter = 0.20;
                visualPalica.Material = Materials.Orange;

                visualPalica.SetName(palica.ime);

                visualModel.Children.Add(visualPalica);
            }

            OnPropertyChanged(nameof(visualModel));
            App.kamera.Prilagodi();
        }

        public void OdstraniVisualElement(string ImeElementa)
        {
            Collection<Visual3D> visualElements = new Collection<Visual3D>();

            foreach (Visual3D visual in visualModel.Children)  if (visual.GetName().Contains(ImeElementa)) visualElements.Add(visual);

            foreach (Visual3D visual in visualElements) visualModel.Children.Remove(visual);
        }

        public Material NovMaterial(string colorHEX)
        {
            //Console.WriteLine(colorHEX.Length != 6);
            //Console.WriteLine(colorHEX.Length != 3);
            if (colorHEX.Length != 6 && colorHEX.Length != 3) return Materials.Black;
            Material material;
            try
            {
                Color color = (Color)ColorHelper.HexToColor("#" + colorHEX);
                material = MaterialHelper.CreateMaterial(color);
                return material;
            }
            catch (Exception e)
            {
                return Materials.Black;
            }

        }
    }
}
