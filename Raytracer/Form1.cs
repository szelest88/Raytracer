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
            bmp.SetPixel(i,j, Color.LightYellow);
            pictureBox1.Image = bmp;

            // let's say... kamera w 0,0,0, patrzy na 0,0,1
            Sphere s1 = new Sphere() // VS hinted me this syntax... I didn't know it.
            {
                center = new Vector3(0, 0, 1),
                radius = 0.4f,
                color = new Vector3(1, 0, 0)
            };
            Sphere s2 = new Sphere() // VS hinted me this syntax... I didn't know it.
            {
                center = new Vector3(0.1f, 0.2f, 1),
                radius = 0.15f,
                color = new Vector3(1, 0f, 0)
            };
            List<Sphere> spheres = new List<Sphere>();
            spheres.Add(s1);
         //   spheres.Add(s2);

            PointLight pointLight = new PointLight(new Vector3(5, 40, 10), new Vector3(1, 1, 1));

            Camera cam = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0),800,800);

            for(int i=-cam.xRes/2;i<cam.xRes/2;i++)
                for(int j = -cam.yRes/2; j < cam.yRes/2; j++)
                {
                    Vector3 targetPoint = cam.getPointFromCenter(
                        2.0f / (float)(cam.xRes) * (float)i,
                        2.0f / (float)(cam.yRes) * (float)j);

                    Ray r = new Ray(cam.position, targetPoint - cam.position);
                    foreach (Sphere sphere in spheres)
                    {
                        Vector3 intersection = sphere.intersects(r);
                        if (intersection != null)
                        {

                            Vector3 normal = s1.calculateNormal(intersection);
                            Vector3 resultColor = new Vector3();
                            Vector3 intersection_minus_cam_pos = intersection - cam.position;

                            float ambientCoeff = 0.2f;
                            float diffuseCoeff = 0.4f;
                            float speculrCoeff = 0.4f;
                            resultColor += new Vector3(1f, 1f, 1f) * ambientCoeff; // "ambient"
                            resultColor += sphere.color * diffuseCoeff *
                                (normal.dot((intersection - pointLight.position).normalized())); // diffuse

                            // specular:
                            Vector3 reflectedVector = (intersection - pointLight.position) - 2 * ((intersection - pointLight.position).dot(normal)) * (normal);
                            reflectedVector = reflectedVector.normalized();

                            resultColor += new Vector3(1f, 1f, 1f) * speculrCoeff *
                                (float)Math.Pow(
                                    reflectedVector.dot(intersection_minus_cam_pos.normalized())
                                    , 10f);

                            if (resultColor.x < 0)
                                resultColor.x = 0;
                            if (resultColor.y < 0)
                                resultColor.y = 0;
                            if (resultColor.z < 0)
                                resultColor.z = 0;


                            bmp.SetPixel(i + cam.xRes / 2, j + cam.yRes / 2,
                                Color.FromArgb(
                                    (int)(resultColor.x * 255.0f),
                                    (int)(resultColor.y * 255.0f),
                                    (int)(resultColor.z * 255.0f)
                                ));
                        }
                    }

                }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }

    public class PointLight
    {
        public Vector3 position;
        public Vector3 color;
        public PointLight(Vector3 position, Vector3 color)
        {
            this.position = position;
            this.color = color;
        }
    }
    public class Camera
    {
        public Vector3 position;
        public Vector3 direction;
        public Vector3 up;
        public int xRes, yRes;

        public Camera(Vector3 position, Vector3 direction, Vector3 up, int xRes, int yRes)
        {
            this.position = position;
            this.direction = direction;
            this.up = up;
            this.xRes = xRes;
            this.yRes = yRes;
        }

        public Vector3 getFirstPoint() // returns the origin translated by 1,dir,up
        {
            return position + direction + up + new Vector3(1, 0, 0);
        }

        public Vector3 getPoint(float x, float y) // returns the first point translated by x,y
        {
            Vector3 firstPoint = getFirstPoint();
            return firstPoint + new Vector3((float)x, (float)y, 0);
        }

        public Vector3 getPointFromCenter(float x, float y) // returns the lookAt point translated by x,y
        {
            Vector3 firstPoint = position + direction;
            return firstPoint + new Vector3((float)x, (float)y, 0);
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
        public Vector3()
        {
            this.x = 0;
            this.y = 0;
            this.z = 0;
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

        public float length()
        {
            return (float)Math.Sqrt(x * x + y * y + z * z);
        }
        public static Vector3 operator * (Vector3 vec, float scalar)
        {
            return new Vector3(vec.x * scalar, vec.y * scalar, vec.z * scalar);
        }
        public static Vector3 operator *(float scalar, Vector3 vec)
        {
            return new Vector3(vec.x * scalar, vec.y * scalar, vec.z * scalar);
        }
        public Vector3 normalized()
        {
            float len = length();
            return new Vector3(x / len, y / len, z / len);
        }

        public float dot(Vector3 vec2)
        {
            return x * vec2.x + y * vec2.y + z * vec2.z;
        }

    public Vector3 cross(Vector3 vec2)
    {
        return new Vector3(
            y*vec2.z-z*vec2.y,
            z*vec2.x-x*vec2.z,
            x*vec2.y-y*vec2.x
            );
    }
    }

    public class Sphere
    {
        public Vector3 center;
        public float radius;

        public Vector3 color;
        public Vector3 calculateNormal(Vector3 point)
        {
            Vector3 res = new Vector3();
            res = (point - center).normalized();
            return res;
        }
        public Vector3 intersects(Ray ray)
        {
            float x0 = ray.begin.x; float y0 = ray.begin.y; float z0 = ray.begin.z;
            Vector3 dirNormalized = ray.direction.normalized();
            float xv = ray.direction.x; float yv = ray.direction.y; float zv = ray.direction.z;
            float xc = center.x; float yc = center.y; float zc = center.z;
            float R = radius;

            float a = xv * xv + yv * yv + zv * zv;
            float b = 2 * (x0*xv-xc*xv+y0*yv-yv*yc+z0*zv-zv*zc);
            float c = x0 * x0 + xc * xc + y0 * y0 + yc * yc + z0 * z0 + zc * zc - 2*(z0 * zc + y0 * yc + z0 * zc) - R * R;

            float delta = b * b - 4 * a * c;

            if (delta == 0)
            {
                float t = -b / (2 * a);
                return new Vector3(x0 + t * xv, y0 + t * yv, z0 + t * zv);
            }
            if (delta > 0)
            {
                // roboczo: zwrócić ten bliższy emiterowi promienia
                float sqrt_delta = (float)Math.Sqrt(delta);
                float t1 = (-b + sqrt_delta) / (2 * a);
                float t2 = (-b - sqrt_delta) / (2 * a);
                if (t1 < t2) // ????
                    return new Vector3(x0 + t1 * xv, y0 + t1 * yv, z0 + t1 * zv);
                else
                    return new Vector3(x0 + t2 * xv, y0 + t2 * yv, z0 + t2 * zv);
            }
            else //delta <0
                return null;


        }
    }

    public class Ray
    {
        public Vector3 begin;
        public Vector3 direction;

        public Ray(Vector3 begin, Vector3 direction)
        {
            this.begin = begin;
            this.direction = direction;
        }
    }


}
