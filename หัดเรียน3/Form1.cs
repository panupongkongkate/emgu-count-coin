using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;


namespace หัดเรียน3
{
    public partial class Form1 : Form
    {
        OpenFileDialog op = new OpenFileDialog();
        Image<Bgr, byte> image1;
        Image<Gray, byte> thould;
        Image<Gray, byte> Binarization; //แปลงภาพไบนารี่
        Image<Gray, byte> cny;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (op.ShowDialog() == DialogResult.OK)
            {
                //แปลงภาพเป็น สีเทา

                image1 = new Image<Bgr, byte>(op.FileName);
                image1 = image1.Resize(1200, 800, Emgu.CV.CvEnum.Inter.Linear, true);
  
                Image<Gray, byte> Gray = image1.Convert<Gray, byte>();
                //----------------------------------------------
                //เบลอภาพด้วย SmoothGaussian
                Gray = Gray.SmoothGaussian(9);
                //ทำการEqualize
                Gray._EqualizeHist();
                imageBox2.Image = Gray;
                //---------------------------------------------
                

                //------
                // การทำBinarization เป็นภาพแบบ ไบนารี่ 
                Binarization = new Image<Gray, byte>(Gray.Width, Gray.Height, new Gray(0));
                //เป็นการทำ Otsu Threshold
                CvInvoke.Threshold(Gray, Binarization, 0, 255, Emgu.CV.CvEnum.ThresholdType.Otsu | Emgu.CV.CvEnum.ThresholdType.Binary);
                //ในlaw บอกว่าต้องใช่Otsu ร่วมกับ Binary ด้วย
                imageBox3.Image = Binarization;
                histogramBox3.ClearHistogram();
                histogramBox3.GenerateHistograms(Gray, 256);
                histogramBox3.Refresh();
                //---------------------------------------------------------
                // หาขอบภาพด้วย canny edge detection
                cny = Binarization.Canny(180,120);
               
                //-----------------การขยายภาพDilate----------------
                cny._Dilate(3);


                // -----ทำการConvolution ภาพ
                
                ConvolutionKernelF convolutionKernelF = new ConvolutionKernelF(3,3);
               
                cny.Convolution(convolutionKernelF);
                
                imageBox1.Image = cny;
                
                //---------------------วาดภาพ ---------------------------------
                

                //Call HoughCircles (Canny included)

                CircleF[] circleFs = cny.HoughCircles(
                    new Gray(180), //cannyThreshold
                    new Gray(450), //accumulatorThreshold
                    6.0, //dp
                    15.0, //minDist
                    5, //minRadius
                    0 //maxRadius
                    )[0];
                // Draw circles
                int outputva = 0 ;//ตัวแปรนับเหรียญ
                Image<Bgr, Byte> imageCircles = new Image<Bgr, byte>(cny.Size) ;
                foreach (CircleF circle in circleFs)
                {
                    imageCircles.Draw(circle, new Bgr(Color.Red), 5);
                    outputva++;
                }
                
                //show 
                imageBox4.Image = imageCircles;
                textBox1.Text = outputva.ToString();
                
            }
        }

        private void histogramBox1_Load(object sender, EventArgs e)
        {

        }

        private void histogramBox3_Load(object sender, EventArgs e)
        {

        }

        private void imageBox2_Click(object sender, EventArgs e)
        {

        }

        private void imageBox3_Click(object sender, EventArgs e)
        {

        }

        private void histogramBox2_Load(object sender, EventArgs e)
        {

        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void imageBox4_Click(object sender, EventArgs e)
        {

        }

        private void imageBox5_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void histogramBox3_Load_1(object sender, EventArgs e)
        {

        }
    }
}
