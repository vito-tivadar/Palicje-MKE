using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using HelixToolkit.Wpf;
using Palicje_MKE.windows;
using Palicje_MKE.lib.MKE;
using Palicje_MKE.lib;
using System.Windows.Media.Media3D;
using System.IO;
using Newtonsoft.Json;



namespace Palicje_MKE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ModelKonstrukcije konstrukcija;

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            konstrukcija = new ModelKonstrukcije();
            posodobiClenek.DodajModelKonstrukcije(konstrukcija);
            posodobiPalico.DodajModelKonstrukcije(konstrukcija);

            viewportModel.Children.Add(konstrukcija.visualModel);

            App.sporocilo.SetTextBlock(ProgramMessageBox);
            App.sporocilo.SetText("Program za preračun paličnih konstrukcij", Colors.CadetBlue, "Izdelal: Vito Tivadar");

            App.kamera.SetViewPort(viewport3D);
            App.kamera.RavninaXY();

        }

        public void OpenGithub(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/vito-tivadar/Palicje-MKE");
        }
        public void OpenIcons8(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://icons8.com/");
        }

        private void CommonCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DodajClenek_Click(object sender, RoutedEventArgs e)
        {
            DodajClenek dc = new DodajClenek();
            if (dc.ShowDialog() == true)
            {
                if (konstrukcija.DodajClenek(dc.clenek))
                {
                    posodobiPalico.Visibility = Visibility.Collapsed;
                    posodobiClenek.Visibility = Visibility.Visible;
                    posodobiClenek.SetClenek(dc.clenek);
                }
                return;
            }

            App.sporocilo.SetText($"Dodajanje členka je bilo preklicano.");
        }

        private void DodajPalico_Click(object sender, RoutedEventArgs e)
        {
            if (konstrukcija.clenki.Count < 2)
            {
                App.sporocilo.SetError("Za dodajanje palice morata biti dodana vsaj 2 členka.");
                return;
            }
            DodajPalico dp = new DodajPalico();
            dp.DodajSeznamImen(konstrukcija.clenki.PridobiImena());
            if (dp.ShowDialog() == true)
            {
                Palica p = new Palica(konstrukcija.clenki.Pridobi(dp.clenek1), konstrukcija.clenki.Pridobi(dp.clenek2));
                if (konstrukcija.DodajPalico(p))
                {
                    posodobiClenek.Visibility = Visibility.Collapsed;
                    posodobiPalico.Visibility = Visibility.Visible;
                    posodobiPalico.SetPalica(p);
                };
                return;
            }
            App.sporocilo.SetText($"Dodajanje palice je bilo preklicano.");
        }

        private void ClearProgramMessage(object sender, RoutedEventArgs e)
        {
            App.sporocilo.SetText("");
        }
        
        private void TestToJSON_Click(object sender, RoutedEventArgs e)
        {
            var test1 = konstrukcija.clenki;
            var test2 = konstrukcija.palice;

            string jsonFile1 = JsonConvert.SerializeObject(test1, Formatting.Indented);
            string jsonFile2 = JsonConvert.SerializeObject(test2, Formatting.Indented);
            File.WriteAllText("./test.json", jsonFile1);
            File.AppendAllText("./test.json", jsonFile2);
        }

        private void helixViewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HelixViewport3D vp = sender as HelixViewport3D;
            var firstHit = vp.Viewport.FindHits(e.GetPosition(vp)).FirstOrDefault();
            if (firstHit == null)
            {
                posodobiClenek.Visibility = Visibility.Collapsed;
                posodobiPalico.Visibility = Visibility.Collapsed;

                App.sporocilo.SetText("null");
                return;
            }

            Type visualType = firstHit.Visual.GetType();
            
            if (visualType == typeof(SphereVisual3D))
            {
                SphereVisual3D sphere = firstHit.Visual as SphereVisual3D;
                App.sporocilo.SetText($"Izbran je členek: {sphere.GetName()}");

                Clenek c = konstrukcija.clenki.Pridobi(sphere.GetName());
                if (c != null)
                {
                    posodobiClenek.SetClenek(c);
                    posodobiPalico.Visibility = Visibility.Collapsed;
                    posodobiClenek.Visibility = Visibility.Visible;
                }
                else
                {
                    //clenek not found
                    //delete visual element
                }
                return;
            }

            if (visualType == typeof(PipeVisual3D))
            {
                

                PipeVisual3D pipe = firstHit.Visual as PipeVisual3D;
                App.sporocilo.SetText($"palica: c1:{pipe.Point1} c2:{pipe.Point2}");

                Palica p = konstrukcija.palice.Pridobi(pipe.Point1, pipe.Point2);
                if (p != null)
                {
                    posodobiPalico.SetPalica(p);
                    posodobiClenek.Visibility = Visibility.Collapsed;
                    posodobiPalico.Visibility = Visibility.Visible;
                }

                // pipe.Point1 = new Point3D(5, 5, 5);
                return;
            }

        }

        private void CameraFit_Click(object sender, RoutedEventArgs e)
        {
            App.kamera.Prilagodi();
        }
    }
}
