using System.Windows.Controls;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace Palicje_MKE.lib.Helix_Toolkit
{
    public class Kamera
    {
        public ProjectionCamera camera;
        public Viewport3D viewport;

        public double CameraAnimationTime { get; set; } = 0;


        public Kamera()
        {

        }

        public void SetViewPort(HelixViewport3D viewport)
        {
            this.camera = viewport.Camera;
            this.viewport = viewport.Viewport;
        }

        public void Prilagodi()
        {
            CameraHelper.FitView(camera, viewport, CameraAnimationTime);
        }

        public void RavninaXY()
        {
            camera.LookDirection = new Vector3D(0, 0, -1);
            camera.UpDirection = new Vector3D(0, 1, 0);
        }

        public void RavninaYZ()
        {
            camera.LookDirection = new Vector3D(-1, 0, 0);
            camera.UpDirection = new Vector3D(0, 0, 1);
        }

        public void RavninaXZ()
        {
            camera.LookDirection = new Vector3D(0, 1, 0);
            camera.UpDirection = new Vector3D(0, 0, 1);
        }

        public void Default()
        {
            camera.LookDirection = new Vector3D(-1, -1, -1);
        }
    }
}
