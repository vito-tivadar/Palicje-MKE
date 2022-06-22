using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.IO;

using Newtonsoft.Json;
using HelixToolkit.Wpf;
using Palicje_MKE.lib.MKE;
using Microsoft.Win32;
using System.Windows;

namespace Palicje_MKE.lib
{
    public class ModelKonstrukcije : NadzorujObjekt
    {
        public Clenki clenki;
        public Palice palice;

        public ModelVisual3D visualModel;
        public ModelVisual3D visualRezultat;

        public ModelKonstrukcije()
        {
            clenki = new Clenki();
            palice = new Palice();

            visualModel = new ModelVisual3D();
            visualRezultat = new ModelVisual3D();
        }

        public bool changed = false;

        public void Resi()
        {
            // disable all buttons

            //dodaj zaporedna števila členkom
            for (int i = 0; i < clenki.Count; i++)
            {
                clenki.DodajPS(clenki[i], i + 1);
            }

            double[,] globalnaTM = new double[3 * palice.Count, 3 * palice.Count];
            double[] globalnaSila = new double[3 * palice.Count];



            //dodajanje lokalnih sil členkov v globalno
            foreach (Clenek clenek in clenki)
            {
                double[] sila = clenek.LokalnaSilaClenka(palice.Count);

                for (int v = 0; v < sila.Length; v++)
                {
                    globalnaSila[v] += sila[v];
                }
            }

            //dodajanje lokalnih togostnih matrik v globalno
            foreach (Palica palica in palice)
            {
                double[,] tm = palica.TogostnaMatrikaElementa(palice.Count);

                for (int v = 0; v < tm.GetLength(0); v++)
                {
                    for (int s = 0; s < tm.GetLength(1); s++)
                    {
                        globalnaTM[v, s] += tm[v, s];
                    }
                }
            }

            // odstrani vrstice in stolpce kjer so členki nepomični
            Collection<int> pomicneStopnje = new Collection<int>();
            for (int i = 0; i < clenki.Count; i++)
            {
                Podpora p = clenki[i].podpora;
                if (!p.X) pomicneStopnje.Add(3 * (i + 1) - 3);
                if (!p.Y) pomicneStopnje.Add(3 * (i + 1) - 2);
                if (!p.Z) pomicneStopnje.Add(3 * (i + 1) - 1);
            }

            double[,] koncnaGlobalnaTM = new double[pomicneStopnje.Count, pomicneStopnje.Count];
            double[] koncnaGlobalnaSila = new double[pomicneStopnje.Count];
            for (int v = 0; v < pomicneStopnje.Count; v++)
            {
                for (int s = 0; s < pomicneStopnje.Count; s++)
                {
                    koncnaGlobalnaTM[v, s] = globalnaTM[pomicneStopnje[v], pomicneStopnje[s]];
                }
                koncnaGlobalnaSila[v] = globalnaSila[pomicneStopnje[v]];
            }

            // Gaussova eliminacija
            for (int i = 0; i < pomicneStopnje.Count; i++)
            {
                for (int v = i + 1; v < pomicneStopnje.Count; v++)
                {
                    double koef = koncnaGlobalnaTM[v, i] / koncnaGlobalnaTM[i, i];
                    for (int s = 0; s < pomicneStopnje.Count; s++)
                    {
                        koncnaGlobalnaTM[v, s] -= koncnaGlobalnaTM[i, s] * koef;
                    }
                    koncnaGlobalnaSila[v] -= koncnaGlobalnaSila[i] * koef;
                }
            }

            //Izračun iskanih vrednosti
            double temp = 0;
            double n = 0;
            double[] x = new double[pomicneStopnje.Count];

            for (int vrstica = pomicneStopnje.Count - 1; vrstica >= 0; vrstica--)
            {
                for (int stolpec = pomicneStopnje.Count - 1; stolpec >= 0; stolpec--)
                {
                    if (vrstica == stolpec)
                    {
                        n = koncnaGlobalnaTM[stolpec, vrstica];
                        break;
                    }
                    else
                    {
                        temp += koncnaGlobalnaTM[vrstica, stolpec] * koncnaGlobalnaSila[stolpec];
                    }
                }
                koncnaGlobalnaSila[vrstica] = (koncnaGlobalnaSila[vrstica] - temp) / n;
                x[vrstica] = koncnaGlobalnaSila[vrstica];
                temp = 0;
            }

            string rezultati = "Pomiki vozlišč:\n";
            for (int i = 0; i < pomicneStopnje.Count; i++)
            {
                int vozlisce = Convert.ToInt32(Math.Floor((decimal)pomicneStopnje[i] / 3)) + 1;

                int ostanek = pomicneStopnje[i] % 3;
                string smer = "";
                if (ostanek == 0) smer = "x";
                if (ostanek == 1) smer = "y";
                if (ostanek == 2) smer = "z";
                rezultati += $"Vozlišče {vozlisce} v smeri {smer}: {x[i].ToString("0.######")} mm\n";
            }

            MessageBox.Show(rezultati, "Rezultati");
        }

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
                foreach (Palica palica in k.Item2)
                {
                    Clenek c1 = clenki.Pridobi(palica.clenek1.koordinate);
                    Clenek c2 = clenki.Pridobi(palica.clenek2.koordinate);

                    Palica p = new Palica(c1, c2, palica.ploscinaPrereza, palica.modulElasticnosti);
                    palice.Dodaj(p);
                }
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
