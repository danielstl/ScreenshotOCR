using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
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
using System.Windows.Shapes;


namespace ScreenshotOCR
{
    /// <summary>
    /// Interaction logic for ScreenshotTaker.xaml
    /// </summary>
    public partial class ScreenshotTaker : Window
    {
        public ScreenshotTaker()
        {
            InitializeComponent();

            var ss = Screenshot.CaptureScreenshot(new System.Drawing.Point(0, 0), new System.Drawing.Size((int) SystemParameters.PrimaryScreenWidth, (int) SystemParameters.PrimaryScreenHeight));

            var file = System.IO.Path.GetTempFileName();
            ss.Save(file, ImageFormat.Png);


            Background = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(file))
            };
        }

        private Point dragStart, dragEnd;
        private bool mouseDown;

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            dragStart = e.GetPosition(this);
            mouseDown = true;
        }

        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            dragEnd = e.GetPosition(this);
            mouseDown = false;
        }

        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown) return;
            dragEnd = e.GetPosition(this);

            debug.Content = dragEnd.ToString();

            regionSelector.Margin = new Thickness(Math.Min(dragStart.X, dragEnd.X), Math.Min(dragStart.Y, dragEnd.Y), 0, 0);
            regionSelector.Width = Math.Max(dragStart.X, dragEnd.X) - regionSelector.Margin.Left;
            regionSelector.Height = Math.Max(dragStart.Y, dragEnd.Y) - regionSelector.Margin.Top;
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}
