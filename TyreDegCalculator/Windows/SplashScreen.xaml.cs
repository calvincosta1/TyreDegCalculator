using System;
using System.Windows;
using System.Windows.Threading;

namespace TyreDegCalculator.Windows
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    public partial class SplashScreen : Window
    {
        // Create DispatcherTimer object
        DispatcherTimer dT = new DispatcherTimer();

        public SplashScreen()
        {
            InitializeComponent();

            this.Topmost = true;

            // Create tick event
            dT.Tick += new EventHandler(dt_Tick);

            // Set timer
            dT.Interval = new TimeSpan(0, 0, 3);

            // Start timer
            dT.Start();
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            // Display MainWindow
            MainWindow mw = new MainWindow();
            mw.Show();

            // Stop timer
            dT.Stop();

            // Close SplashScreen window
            this.Close();
        }
    }
}