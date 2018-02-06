using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenshotOCR
{
    /// <summary>
    /// Interaction logic for ActionButtons.xaml
    /// </summary>
    public partial class ActionButtons : UserControl
    {
        public event EventHandler OnOCRButtonClick, OnScreenshotButtonClick, OnClipboardButtonClick, OnRetakeButtonClick, OnCloseButtonClick;

        private void retakeButton_Click(object sender, RoutedEventArgs e)
        {
            OnRetakeButtonClick?.Invoke(sender, e);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            OnCloseButtonClick?.Invoke(sender, e);
        }

        private void clipboardButton_Click(object sender, RoutedEventArgs e)
        {
            OnClipboardButtonClick?.Invoke(sender, e);
        }

        private void ocrButton_Click(object sender, RoutedEventArgs e)
        {
            OnOCRButtonClick?.Invoke(sender, e);
        }

        private void screenshotButton_Click(object sender, RoutedEventArgs e)
        {
            OnScreenshotButtonClick?.Invoke(sender, e);
        }

        public ActionButtons()
        {
            InitializeComponent();
        }
    }
}
