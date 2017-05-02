using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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

        void Render(int resolution)
        {
            Bitmap bmp = new Bitmap(resolution, resolution,System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            
            Sphere s1 = new Sphere() // VS hinted me this syntax... I didn't know it.
            {
                center = new Vector3(0, 0, 1),
                radius = 0.2f,
                color = new Vector3(1, 0, 0)
            };
            Sphere s2 = new Sphere()
            {
                center = new Vector3(0.15f, 0.2f, 1.1f),
                radius = 0.15f,
                color = new Vector3(1, 0f, 1)
            };

            List<Sphere> spheres = new List<Sphere>();

            spheres.Add(s1);
            spheres.Add(s2);

            PointLight pointLight = new PointLight(new Vector3(5, 40, 10), new Vector3(1, 1, 1));

            Camera cam = new Camera(new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 1, 0), resolution, resolution);

            Vector3 oneoneone = new Vector3(1, 1, 1);

            for (int i = -cam.xRes/2 ; i < cam.xRes/2; i++)
                for (int j = -cam.yRes/2; j < cam.yRes/2; j++)
                {
                    Vector3 targetPoint = cam.GetPointFromCenter(
                        2f / (float)(cam.xRes) * (float)i,
                        2f / (float)(cam.yRes) * (float)j);

                    Ray r = new Ray(cam.position, targetPoint - cam.position);
                    foreach (Sphere sphere in spheres)
                    {
                        Vector3 intersection = sphere.Intersects(r);
                        if (intersection != null)
                        {

                            Vector3 normal = s1.CalculateNormal(intersection);
                            Vector3 resultColor = oneoneone;
                            Vector3 intersection_minus_cam_pos = intersection - cam.position;

                            float ambientCoeff = 0.2f; float diffuseCoeff = 0.4f; float speculrCoeff = 0.4f;

                            resultColor *= ambientCoeff; // initialized with 1,1,1 => ambient
                            Vector3 intersectionToPointLight = (intersection - pointLight.position);
                            resultColor += sphere.color * diffuseCoeff *
                                (normal.Dot(intersectionToPointLight.Normalized())); // diffuse

                            // specular:
                            Vector3 reflectedVector = intersectionToPointLight - 2 * (intersectionToPointLight.Dot(normal)) * (normal);
                            reflectedVector = reflectedVector.Normalized();

                            resultColor += oneoneone * speculrCoeff *
                                (float)Math.Pow(
                                    reflectedVector.Dot(intersection_minus_cam_pos.Normalized())
                                    , 40f);

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

            pictureBox1.Image = resolution==1600?DownSampled(bmp):bmp;
        }
        unsafe Bitmap DownSampled(Bitmap bmp)
        {
            Bitmap ret = new Bitmap(bmp.Width/2, bmp.Height/2, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData data = ret.LockBits(new Rectangle(0,0,ret.Width,ret.Height),ImageLockMode.ReadWrite,PixelFormat.Format24bppRgb);
            IntPtr begin = data.Scan0;
            int bytes = Math.Abs(data.Stride) * ret.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            System.Runtime.InteropServices.Marshal.Copy(begin, rgbValues, 0, bytes);
            int count = 0;
            for (int i = 0; i < bmp.Width; i++)
                for (int j = 0; j < bmp.Height; j++) {
                    if (i % 2 == 1 && j % 2 == 1) {
                        
                        Color c00 = bmp.GetPixel(j, i);
                        Color c10 = bmp.GetPixel(j-1, i);
                        Color c01 = bmp.GetPixel(j, i-1);
                        Color c11 = bmp.GetPixel(j-1, i-1);

                        Color res = Color.FromArgb(
                            ((int)c00.R + (int)c01.R + (int)c10.R + (int)c11.R) / 4,
                            ((int)c00.G + (int)c01.G + (int)c10.G + (int)c11.G) / 4,
                            ((int)c00.B + (int)c01.B + (int)c10.B + (int)c11.B) / 4);
                        
                        rgbValues[count] = res.B;
                        rgbValues[count+1] = res.G;
                        rgbValues[count+2] = res.R;
                        count += 3;
                    }
                }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, begin, bytes);
            ret.UnlockBits(data);

            return ret;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Render(800);
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void RenderButton_Click(object sender, EventArgs e)
        {
            double time0 = DateTime.Now.TimeOfDay.TotalMilliseconds;
            Render(800);
            double time1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
            System.Console.WriteLine("" + (time1 - time0));
        }

        private void RenderAntialiased_Click(object sender, EventArgs e)
        {
            double time0 = DateTime.Now.TimeOfDay.TotalMilliseconds;
            Render(1600);
            double time1 = DateTime.Now.TimeOfDay.TotalMilliseconds;
            System.Console.WriteLine("" +( time1-time0));
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

        public Vector3 GetFirstPoint() // returns the origin translated by 1,dir,up
        {
            return position + direction + up*0.5f + new Vector3(0.5f, 0, 0);
        }
        //FIXME: uglyyy
        public Vector3 GetPoint(float x, float y) // returns the first point translated by x,y
        {
            Vector3 firstPoint = GetFirstPoint();
            firstPoint.x += x;
            firstPoint.y += y;
            return firstPoint;
        }

        public Vector3 GetPointFromCenter(float x, float y) // returns the lookAt point translated by x,y
        {
            Vector3 firstPoint = position + direction;
            firstPoint.x += x / 2.0f;
            firstPoint.y += y / 2.0f;

            return firstPoint;
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

        public float Length()
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
        public Vector3 Normalized()
        {
            float len = Length();
            return new Vector3(x / len, y / len, z / len);
        }

        public float Dot(Vector3 vec2)
        {
            return x * vec2.x + y * vec2.y + z * vec2.z;
        }

    public Vector3 Cross(Vector3 vec2)
    {
        return new Vector3(
            y*vec2.z-z*vec2.y,
            z*vec2.x-x*vec2.z,
            x*vec2.y-y*vec2.x
            );
    }
    }

    //TODO: implement
    public class Plane
    {
        public Vector3 center;
        public Vector3 normal;

        public Vector3 Intersects(Ray ray)
        {
            Vector3 v = new Vector3();

            return v;

        }
    }
    public class Sphere
    {
        public Vector3 center;
        public float radius;

        public Vector3 color;
        public Vector3 CalculateNormal(Vector3 point)
        {
            return (point - center).Normalized();
            
        }
        public Vector3 Intersects(Ray ray)
        {
            float x0 = ray.begin.x; float y0 = ray.begin.y; float z0 = ray.begin.z;
            float xv = ray.direction.x; float yv = ray.direction.y; float zv = ray.direction.z;
            float xc = center.x; float yc = center.y; float zc = center.z;
            float R = radius;

            float a = xv * xv + yv * yv + zv * zv;
            float b = 2 * ( xv*(x0-xc) + yv*(y0-yc) + zv*(z0-zc));
            float c = x0 * x0 + xc * xc + y0 * y0 + yc * yc + z0 * z0 + zc * zc - 2*(x0 * xc + y0 * yc + z0 * zc) - R * R;

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
                if (t1 > t2) // ????
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
