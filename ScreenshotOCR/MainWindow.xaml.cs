using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ScreenshotOCR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void screenshotButton_Click(object sender, RoutedEventArgs e)
        {
            Opacity = 0;
            Hide();

            var timer = new DispatcherTimer();
            timer.Tick += (ss, ee) =>
            {
                timer.Stop();
                new ScreenshotTaker().ShowDialog();
                Opacity = 1;
                Show();
            };
            timer.Start();
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            //todo
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
