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
using System.Windows.Media.Animation;
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

            loadingPanel.Visibility = Visibility.Hidden;
            contentGrid.Visibility = Visibility.Visible;
        }

        public OCRResultsWindow(Task<string> results)
        {
            InitializeComponent();

            AwaitResults(results);
        }

        private async void AwaitResults(Task<string> results)
        {
            string r = await results;

            resultsTextBox.Text = r;

            loadingPanel.BeginAnimation(OpacityProperty, new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.1)));
            contentGrid.Visibility = Visibility.Visible;
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

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(resultsTextBox.Text);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
