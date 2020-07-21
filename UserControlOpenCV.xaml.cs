using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppTest
{
    /// <summary>
    /// Interaction logic for UserControlOpenCV.xaml
    /// </summary>
    public partial class UserControlOpenCV : UserControl
    {
        Image<Bgr, byte> img;
        Image<Gray, byte> gray;
        int RT;
        int GT;
        int BT;

        public UserControlOpenCV()
        {
            InitializeComponent();
        }

        private void Button_Click_Upload(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openPic = new OpenFileDialog();
            if (openPic.ShowDialog() == true)
            {
                img = new Image<Bgr, byte>(openPic.FileName);
                gray = img.Convert<Gray, byte>();

                myImage.Source = ToBitmapSource(img);
                myGrayImage.Source = ToBitmapSource(gray);
            }
        }

        private void Button_Click_Detect(object sender, RoutedEventArgs e)
        {
            if (gray == null) return;
            CircleF[] circles = gray.HoughCircles(
                new Gray(100),
                new Gray(120),
                5.0, //Resolution of the accumulator used to detect centers of the circles
                50.0, //min distance 
                70, //min radius
                100 //max radius
                )[0]; //Get the circles from the first channel
            Image<Bgr, byte> imageCircles = img.Copy();
            foreach (CircleF circle in circles)
            {
                imageCircles.Draw(circle, new Bgr(System.Drawing.Color.Yellow), 5);
            }
            myImage.Source = ToBitmapSource(imageCircles);
        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        /// <summary>
        /// Convert an IImage to a WPF BitmapSource. The result can be used in the Set Property of Image.Source
        /// </summary>
        /// <param name="image">The Emgu CV Image</param>
        /// <returns>The equivalent BitmapSource</returns>
        public static BitmapSource ToBitmapSource(Image<Bgr, byte> image)
        {
            using (System.Drawing.Bitmap source = image.AsBitmap())
            {
                return tbs(source);
            }
        }
        public static BitmapSource ToBitmapSource(Image<Gray, byte> image)
        {
            using (System.Drawing.Bitmap source = image.AsBitmap())
            {
                return tbs(source);
            }
        }

        public static BitmapSource tbs(System.Drawing.Bitmap source)
        {
            IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

            BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                ptr,
                IntPtr.Zero,
                Int32Rect.Empty,
                System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

            DeleteObject(ptr); //release the HBitmap
            return bs;
        }

        private void Slider_ValueChanged_R(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            RT = (int)slider.Value;
            UpdateThreshold();
        }

        private void Slider_ValueChanged_G(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            GT = (int)slider.Value;
            UpdateThreshold();
        }

        private void Slider_ValueChanged_B(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var slider = sender as Slider;
            BT = (int)slider.Value;
            UpdateThreshold();
        }

        public void UpdateThreshold()
        {
            gray = img.ThresholdBinary(new Bgr(BT, GT, RT), new Bgr(255, 255, 255)).Convert<Gray, byte>(); ;
            
            //CvInvoke.Threshold(gray, gray, 100, 255, ThresholdType.Otsu | ThresholdType.Binary);
            myGrayImage.Source = ToBitmapSource(gray.ThresholdBinary(new Gray(30), new Gray(255)));
        }

        private void Button_Click_Contours(object sender, RoutedEventArgs e)
        {
            //Mat hierachy = new Mat();
            //VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            //Image<Gray, byte> canny = gray.CopyBlank();
            //CvInvoke.Canny(gray, canny, 10, 100);
            //CvInvoke.FindContours(canny, contours, hierachy, RetrType.Tree, ChainApproxMethod.ChainApproxNone);
            //Image<Gray, byte> draw = gray.CopyBlank();
            //for (int i = 0; i < contours.Size; i++)
            //{
            //    CvInvoke.DrawContours(draw, contours, i, new MCvScalar(0, 100, 255), 3, LineType.EightConnected, hierachy);
            //    draw.Draw(contours[i].ToArray(), new Gray(0), 5);
            //}

            //CvInvoke.Threshold(gray, gray, 100, 255, ThresholdType.Otsu | ThresholdType.Binary);

            //myGrayImage.Source = ToBitmapSource(canny);
            SimpleBlobDetectorParams blobParams = new SimpleBlobDetectorParams();
            //blobParams.MinThreshold = 0;
            //blobParams.MaxThreshold = 100;

            //blobParams.FilterByArea = true;
            blobParams.MinArea = 10;
            blobParams.MaxArea = 50;

            SimpleBlobDetector detector = new SimpleBlobDetector(blobParams);
            VectorOfKeyPoint keyPoints = new VectorOfKeyPoint();
            detector.DetectRaw(gray.ThresholdBinary(new Gray(30), new Gray(255)), keyPoints);
            
            //Mat im_with_keypoints = new Mat();
            Bgr color = new Bgr(0, 0, 255);
            //Features2DToolbox.DrawKeypoints(gray, keyPoints, gray, color, Features2DToolbox.KeypointDrawType.DrawRichKeypoints);

            myGrayImage.Source = ToBitmapSource(gray);
        }
    }
}
