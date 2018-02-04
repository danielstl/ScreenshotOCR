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
        private bool mouseDown, screenshotTaken;
        private System.Windows.Shapes.Path canvasPath;

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (screenshotTaken) return;
            dragStart = dragEnd = e.GetPosition(this);
            mouseDown = true;
            DrawCanvas();
            CaptureMouse();
        }

        private void Window_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (screenshotTaken) return;
            dragEnd = e.GetPosition(this);
            mouseDown = false;
            DrawCanvas();

            HandleScreenshot();
        }

        private void Window_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown || screenshotTaken) return;
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

        private void HandleScreenshot()
        {
            screenshotTaken = true;

            ReleaseMouseCapture();

            Cursor = Cursors.Wait;

            double mx = Math.Max(dragStart.X, dragEnd.X);
            double my = Math.Max(dragStart.Y, dragEnd.Y);
            actions.SetValue(Canvas.LeftProperty, mx);
            actions.SetValue(Canvas.TopProperty, my);

            actions.Opacity = 0;
            actions.Visibility = Visibility.Visible;

            actions.BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.1)));

            int width = (int) (Math.Max(dragStart.X, dragEnd.X) - Math.Min(dragStart.X, dragEnd.X));
            int height = (int) (Math.Max(dragStart.Y, dragEnd.Y) - Math.Min(dragStart.Y, dragEnd.Y));

            int scale = 8;

            var img = new Bitmap(width*scale, height*scale);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(screenshot, new System.Drawing.Rectangle(0, 0, width*scale, height*scale), new System.Drawing.Rectangle((int) Math.Min(dragStart.X, dragEnd.X), (int) Math.Min(dragStart.Y, dragEnd.Y), width, height), GraphicsUnit.Pixel);
            }

            img.Save(@"C:\Users\Daniel\Pictures\testimg.bmp");
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                        using (var page = engine.Process(PixConverter.ToPix(img)))
                        {
                        Cursor = Cursors.Arrow;
                        var text = page.GetText();

                        MessageBox.Show("Found the following text:\n\n" + text);
                            /*msg += ("Text (iterator):");
                            using (var iter = page.GetIterator())
                            {
                                iter.Begin();

                                do
                                {
                                    do
                                    {
                                        do
                                        {
                                            do
                                            {
                                                if (iter.IsAtBeginningOf(PageIteratorLevel.Block))
                                                {
                                                    msg += ("<BLOCK>\n");
                                                }

                                                msg += (iter.GetText(PageIteratorLevel.Word));
                                                msg += (" ");

                                                if (iter.IsAtFinalOf(PageIteratorLevel.TextLine, PageIteratorLevel.Word))
                                                {
                                                msg += "\n";
                                                }
                                            } while (iter.Next(PageIteratorLevel.TextLine, PageIteratorLevel.Word));

                                            if (iter.IsAtFinalOf(PageIteratorLevel.Para, PageIteratorLevel.TextLine))
                                            {
                                            msg += "\n";
                                            }
                                        } while (iter.Next(PageIteratorLevel.Para, PageIteratorLevel.TextLine));
                                    } while (iter.Next(PageIteratorLevel.Block, PageIteratorLevel.Para));
                                } while (iter.Next(PageIteratorLevel.Block));
                        }*/
                    }
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Console.WriteLine("Unexpected Error: " + e.Message);
                Console.WriteLine("Details: ");
                Console.WriteLine(e.ToString());
            }
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
