using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace ScreenshotOCR
{
    class Screenshot
    {
        public static Bitmap CaptureScreenshot(Point pos, Size size)
        {
            var screen = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb);
            Graphics.FromImage(screen).CopyFromScreen(pos, pos, size, CopyPixelOperation.SourceCopy);

            return screen;
        }
    }
}
