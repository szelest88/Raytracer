using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Raytracer
{
 

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(800, 800);
            for(int i=0;i<800;i++)
                for(int j=0;j<800;j++)
            bmp.SetPixel(i,j, Color.DarkGray);
            pictureBox1.Image = bmp;

            // let's say... kamera w 0,0,0, patrzy na 0,0,1
            Sphere s1 = new Sphere() // VS hinted me this syntax... I didn't know it.
            {
                center = new Vector3(0, 0, 1),
                radius = 0.2f
            };

            Camera cam = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0));

        }
    }

    public class Vector3
    {
        public float x, y, z;
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public class Sphere
    {
        public Vector3 center;
        public float radius;
    }

    public class Ray
    {
        Vector3 begin;
        Vector3 direction;

        public Ray(Vector3 begin, Vector3 direction)
        {
            this.begin = begin;
            this.direction = direction;
        }
    }

    public class Camera
    {
        public Vector3 position;
        public Vector3 direction;
        public Vector3 up;

        public Camera(Vector3 position, Vector3 direction, Vector3 up)
        {
            this.position = position;
            this.direction = direction;
            this.up = up;
        }

    }
}
