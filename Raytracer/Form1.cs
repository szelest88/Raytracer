﻿using System;
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

        public static Vector3 operator +(Vector3 one, Vector3 other)
        {
            return new Vector3(one.x + other.x, one.y+ other.y, one.z + other.z);
        }
        public static Vector3 operator -(Vector3 one, Vector3 other)
        {
            return new Vector3(one.x - other.x, one.y - other.y, one.z - other.z);
        }
        public override String ToString()
        {
            return ("" + x + ", " + y + ", " + z);
        }
    }

    public class Sphere
    {
        public Vector3 center;
        public float radius;

        public bool intersects(Ray ray)
        {
            float x0 = ray.begin.x; float y0 = ray.begin.y; float z0 = ray.begin.z;
            float xv = ray.direction.x; float yv = ray.direction.y; float zv = ray.direction.z;
            float xc = center.x; float yc = center.y; float zc = center.z;
            float R = radius;

            float a = xv * xv + yv * yv + zv * zv;
            float b = 2 * (x0*xv-xc*xv+y0*yv-yv*yc+z0*zv-zv*zc);
            float c = x0 * x0 + xc * xc + y0 * y0 + yc * yc + z0 * z0 + zc * zc - 2*(z0 * zc + y0 * yc + z0 * zc) - R * R;

            float delta = b * b - 4 * a * c;

            if (delta >= 0)
                return true;
           
            return false;

        }
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
