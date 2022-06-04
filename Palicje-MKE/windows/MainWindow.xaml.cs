using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using HelixToolkit.Wpf;
using Palicje_MKE.windows;
using System.Windows.Media.Media3D;



namespace Palicje_MKE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Model3DGroup konstrukcija { get; set; } = new Model3DGroup();

        public MainWindow()
        {
            DataContext = this;
            InitializeComponent();

            viewport2D.Camera.ChangeDirection(new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 0);
            viewport3D.Camera.ChangeDirection(new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 0);
            viewport3D_rezultati.Camera.ChangeDirection(new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), 0);


            /*
            MeshBuilder builder = new MeshBuilder();
            builder.AddSphere(new Point3D(3, 3, 3), 1);
            
            MeshGeometry3D mesh = builder.ToMesh(true);

            Material green = MaterialHelper.CreateMaterial(Colors.Red);
            Material white = MaterialHelper.CreateMaterial(Colors.White);

            konstrukcija.Children.Add(new GeometryModel3D { Geometry = mesh, Material = green, BackMaterial = white });
            */

            MeshBuilder b = new MeshBuilder();
            b.AddNode(new Point3D(3, 3, 3), new Vector3D(1, 1, 1), );

        }

        private void CommonCommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void DodajVozlisce_Click(object sender, RoutedEventArgs e)
        {
            poljeZaSpreminjanje_grid.Children.Add(new ClenekControl());
        }
    }
}
