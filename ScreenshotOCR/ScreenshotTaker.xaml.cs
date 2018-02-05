using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Tesseract;

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

            screenshot = Screenshot.CaptureScreenshot(new System.Drawing.Point(0, 0), new System.Drawing.Size((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight));

            var file = System.IO.Path.GetTempFileName();
            screenshot.Save(file, System.Drawing.Imaging.ImageFormat.Png);

            Background = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri(file))
            };

            DrawCanvas();
        }

        private Bitmap screenshot;
        private System.Windows.Point dragStart, dragEnd;
        private bool mouseDown, screenshotRegionSelected;
        private System.Windows.Shapes.Path canvasPath;
        private string ocrResult;

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (screenshotRegionSelected) return;
            dragStart = dragEnd = e.GetPosition(this);
            mouseDown = true;
            DrawCanvas();
            CaptureMouse();
        }

        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (screenshotRegionSelected) return;
            dragEnd = e.GetPosition(this);
            mouseDown = false;
            DrawCanvas();

            HandleScreenshot();
        }

        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown || screenshotRegionSelected) return;
            dragEnd = e.GetPosition(this);

            DrawCanvas();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }

        private void actions_OnCloseButtonClick(object sender, EventArgs e)
        {

        }

        private void actions_OnOCRButtonClick(object sender, EventArgs e)
        {
            //todo display loading msg or whatever
            if (ocrResult == null)
            {
                MessageBox.Show("ocr not complete");
                return;
            }
            new OCRResultsWindow(ocrResult).Show();
        }

        private void actions_OnRetakeButtonClick(object sender, EventArgs e)
        {

        }

        private void actions_OnScreenshotButtonClick(object sender, EventArgs e)
        {

        }

        private async void HandleScreenshot()
        {
            screenshotRegionSelected = true;

            ReleaseMouseCapture();

            double mx = Math.Max(dragStart.X, dragEnd.X);
            double my = Math.Max(dragStart.Y, dragEnd.Y);
            actions.SetValue(Canvas.LeftProperty, mx - actions.ActualWidth);
            actions.SetValue(Canvas.TopProperty, my + 2);

            actions.Opacity = 0;
            actions.Visibility = Visibility.Visible;

            actions.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1)));

            ocrResult = await Screenshot.PerformOcr(screenshot, dragStart, dragEnd);
        }

        private void DrawCanvas()
        {
            debug.Content = mouseDown + "";
            var path = new System.Windows.Shapes.Path();
            path.Stroke = new SolidColorBrush(Colors.DarkGray);
            path.StrokeThickness = 2;
            path.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(100, 255, 255, 255));
            path.Data = new CombinedGeometry(GeometryCombineMode.Exclude, new RectangleGeometry(new System.Windows.Rect(-1, -1, SystemParameters.PrimaryScreenWidth + 2, SystemParameters.PrimaryScreenHeight + 2)), new RectangleGeometry(new System.Windows.Rect(dragStart, dragEnd)));
            imageCanvas.Children.Remove(canvasPath);
            canvasPath = path;

            imageCanvas.Children.Add(path);
        }
    }
}
