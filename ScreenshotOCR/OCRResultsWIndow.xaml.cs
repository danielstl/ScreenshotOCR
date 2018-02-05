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
using System.Windows.Shapes;

namespace ScreenshotOCR
{
    /// <summary>
    /// Interaction logic for OCRResultsWindow.xaml
    /// </summary>
    public partial class OCRResultsWindow : Window
    {
        public OCRResultsWindow(string results)
        {
            InitializeComponent();

            resultsTextBox.Text = results;
        }

        private void pinButton_Click(object sender, RoutedEventArgs e)
        {
            Topmost = !Topmost;
        }

        private void resultsTextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
