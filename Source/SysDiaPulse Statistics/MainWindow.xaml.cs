using SysDiaPulseCore.Common.Filtres;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using static SysDiaPulseCore.Methods.Rgb;

namespace SysDiaPulse_Statistics
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                var hardFillFilter = new HardFill(new ColorRGB(0, 125, 255), new ColorRGB(255, 255, 255), 0.70f);
                byte[,,] imageRaw = BitmapToByteRgbQ(LoadBitmap("image.jpg"));
                hardFillFilter.Apply(ref imageRaw);
                var bitmapImage = RgbToBitmapQ(imageRaw);
                MemoryStream ms = new MemoryStream();
                bitmapImage.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();
                Image1.Source = image;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
