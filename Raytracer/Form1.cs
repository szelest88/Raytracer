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
            Bitmap bmp = new Bitmap(800, 600);
            for(int i=0;i<800;i++)
                for(int j=0;j<600;j++)
            bmp.SetPixel(i,j, Color.Red);
            pictureBox1.Image = bmp;
        }
    }
}
