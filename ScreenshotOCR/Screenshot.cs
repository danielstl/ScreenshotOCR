using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows;
using Tesseract;

namespace ScreenshotOCR
{
    class Screenshot
    {
        public static Bitmap CaptureScreenshot(System.Drawing.Point pos, System.Drawing.Size size)
        {
            var screen = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            Graphics.FromImage(screen).CopyFromScreen(pos, pos, size, CopyPixelOperation.SourceCopy);

            return screen;
        }

        public static async Task<string> PerformOcr(Bitmap screenshot, System.Windows.Point dragStart, System.Windows.Point dragEnd)
        {
            var ds = dragStart;
            var de = dragEnd;

            return await Task.Run(() =>
             {
                 int width = (int)(Math.Max(ds.X, de.X) - Math.Min(ds.X, de.X));
                 int height = (int)(Math.Max(ds.Y, de.Y) - Math.Min(ds.Y, de.Y));

                 int scale = 8;

                 var img = new Bitmap(width * scale, height * scale);
                 using (Graphics g = Graphics.FromImage(img))
                 {
                     g.DrawImage(screenshot, new Rectangle(0, 0, width * scale, height * scale), new Rectangle((int)Math.Min(ds.X, de.X), (int)Math.Min(ds.Y, de.Y), width, height), GraphicsUnit.Pixel);
                 }

                 var text = "";

                 try
                 {
                     using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                     {
                         using (var page = engine.Process(PixConverter.ToPix(img)))
                         {
                             text = page.GetText();
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     Debug.WriteLine($"Exception performing OCR: {ex.Message}");
                     Debug.WriteLine(ex.StackTrace);
                 }

                 return text;
             });
        }
    }
}
