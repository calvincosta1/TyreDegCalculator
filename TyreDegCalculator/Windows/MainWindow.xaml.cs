using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using TyreDegCalculator.Models;
using static TyreDegCalculator.Models.TyreModel;

namespace TyreDegCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // Global variables
        string APIKey = "08aabd9e234b08ab0f900e72583ef820";
        List<Models.TrackModel> Tracks = new List<Models.TrackModel>();
        Models.TrackModel trackModel;
        string FLTyre, FRTyre, RLTyre, RRTyre;
        float temp;
        Tyres tyres;

        // Window loaded event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Calling loadData method
            loadData();
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        // Method to get data from .xml file and .txt and populate comboboxes
        private void loadData()
        {
            // Getting and splitting data from .txt file
            using (StreamReader sr = new StreamReader("TrackDegradationCoefficients.txt"))
            {
                while (sr.Peek() >= 0)
                {
                    string str;
                    string[] strArray1;
                    string[] strArray2;
                    str = sr.ReadLine();

                    strArray1 = str.Split('|');
                    strArray2 = strArray1[2].Split(',');
                    int[] values = new int[strArray2.Length];
                    TrackModel currentTrack = new TrackModel();
                    currentTrack.Name = strArray1[0];
                    currentTrack.Location = strArray1[1];

                    for (int i = 0; i < strArray2.Length; i++)
                    {
                        values[i] = Convert.ToInt32(strArray2[i]);
                    }

                    currentTrack.Values = values;

                    Tracks.Add(currentTrack);
                }
            }

            // Populating track combobox
            TrackCombo.ItemsSource = Tracks;

            // Deserializing data from .xml file
            string fileName = "TyresXML.xml";
            var stream = new FileStream(fileName, FileMode.Open);
            tyres = (Tyres)new XmlSerializer(typeof(Tyres)).Deserialize(stream);

            // Populating tyre comboboxes
            FrontLeftCombo.ItemsSource = tyres.Tyress.Where(i => i.Placement == "FL");
            FrontRightCombo.ItemsSource = tyres.Tyress.Where(i => i.Placement == "FR");
            RearLeftCombo.ItemsSource = tyres.Tyress.Where(i => i.Placement == "RL");
            RearRightCombo.ItemsSource = tyres.Tyress.Where(i => i.Placement == "RR");
        }

        // Method to call OpenWeatherMap API
        private async Task getWeatherAsync(string city)
        {
            // Error handling
            try
            {
                // Fetching weather
                using (WebClient web = new WebClient())
                {
                    string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&appid={1}", city, APIKey);
                    var json = web.DownloadString(url);
                    WeatherModel.root Info = await Task.Run(() => JsonConvert.DeserializeObject<WeatherModel.root>(json));

                    temp = Info.main.temp;
                    TempTxtBox.Text = temp.ToString();
                }
            }
            catch (Exception)
            {
                // Messagebox to display error message to user
                MessageBox.Show("Error fetching weather!\nPlease check your internet connectivity!");
            }
        }

        // Selection changed method for Track combobox
        private async void TrackCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedTrack = ((TrackModel)TrackCombo.SelectedItem).Name;

            trackModel = Tracks.Where(track => track.Name == selectedTrack).ToList()[0];

            await getWeatherAsync(trackModel.Location);

            await Task.Run(() => calculations());
        }

        // Selection changed method for Front Left Tyre combobox
        private async void FrontLeftCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FLTyre = ((TyreModel.Tyre)FrontLeftCombo.SelectedItem).Name;
            await Task.Run(() => calculations());
        }

        // Selection changed method for Front Right Tyre combobox
        private async void FrontRightCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            FRTyre = ((TyreModel.Tyre)FrontRightCombo.SelectedItem).Name;
            await Task.Run(() => calculations());
        }

        // Selection changed method for Rear Left Tyre combobox
        private async void RearLeftCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            RLTyre = ((TyreModel.Tyre)RearLeftCombo.SelectedItem).Name;
            await Task.Run(() => calculations());
        }

        // Selection changed method for Rear Right Tyre combobox
        private async void RearRightCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            RRTyre = ((TyreModel.Tyre)RearRightCombo.SelectedItem).Name;
            await Task.Run(() => calculations());
        }

        // Method to do calculations
        private void calculations()
        {
            // Doing some error handling to ensure all options are filled before doing calculations
            if (trackModel == null || FLTyre == null || FRTyre == null || RLTyre == null || RRTyre == null)
            {
                return;
            }

            // Setting cursor to wait cursor for user feeback while performing calculations
            this.Dispatcher.Invoke(() =>
            {
                this.Cursor = Cursors.AppStarting;
            });

            // Arrays to hold values of PointTyreDeg calculations
            float[] FLCalculations = new float[trackModel.Values.Length];
            float[] FRCalculations = new float[trackModel.Values.Length];
            float[] RLCalculations = new float[trackModel.Values.Length];
            float[] RRCalculations = new float[trackModel.Values.Length];

            // Loop for calculating PointTyreDeg
            for (int i = 0; i < trackModel.Values.Length; i++)
            {
                var FLTyreChoice = tyres.Tyress.Where(tyre => tyre.Name == FLTyre).ToList()[0];
                var FRTyreChoice = tyres.Tyress.Where(tyre => tyre.Name == FRTyre).ToList()[0];
                var RLTyreChoice = tyres.Tyress.Where(tyre => tyre.Name == RLTyre).ToList()[0];
                var RRTyreChoice = tyres.Tyress.Where(tyre => tyre.Name == RRTyre).ToList()[0];

                float FLTyrePointDeg = calculatePointTyreDeg(trackModel.Values[i], FLTyreChoice);
                float FRTyrePointDeg = calculatePointTyreDeg(trackModel.Values[i], FRTyreChoice);
                float RLTyrePointDeg = calculatePointTyreDeg(trackModel.Values[i], RLTyreChoice);
                float RRTyrePointDeg = calculatePointTyreDeg(trackModel.Values[i], RRTyreChoice);

                FLCalculations[i] = FLTyrePointDeg;
                FRCalculations[i] = FRTyrePointDeg;
                RLCalculations[i] = RLTyrePointDeg;
                RRCalculations[i] = RRTyrePointDeg;
            }

            // Calculating average and rounding to 2 decimal places
            float FLaverage = (float)Math.Round(calculateAve(FLCalculations) * 100f)/ 100f;
            float FRaverage = (float)Math.Round(calculateAve(FRCalculations) * 100f) / 100f;
            float RLaverage = (float)Math.Round(calculateAve(RLCalculations) * 100f) / 100f;
            float RRaverage = (float)Math.Round(calculateAve(RRCalculations) * 100f) / 100f;

            // Calculating range and rounding to 2 decimal places
            float FLrange = (float)Math.Round(calculateRange(FLCalculations) * 100f) / 100f;
            float FRrange = (float)Math.Round(calculateRange(FRCalculations) * 100f) / 100f;
            float RLrange = (float)Math.Round(calculateRange(RLCalculations) * 100f) / 100f;
            float RRrange = (float)Math.Round(calculateRange(RRCalculations) * 100f) / 100f;

            // Clearing and filling textboxes
            clearTextboxes();
            fillTextboxes(FLaverage, FRaverage, RLaverage, RRaverage, FLrange, FRrange, RLrange, RRrange);
            colourTextboxes(FLaverage, FRaverage, RLaverage, RRaverage, FLrange, FRrange, RLrange, RRrange);

            // Setting cursor back to default arrow
            this.Dispatcher.Invoke(() =>
            {
                this.Cursor = Cursors.Arrow;
            });
        }

        // Method to calculate Point Tyre Degredation from given formula PointTyreDeg = (TrackDegPoint * Temp) / TyreCoefficient
        private float calculatePointTyreDeg(int trackDegPoint, Tyre tyre)
        {
            // Percentage to apply to degredation coefficient according to tyre manufacturer
            int percent = 0;
            if (tyre.Name.Contains("Soft"))
            {
                percent = 80;
            }
            else if (tyre.Name.Contains("Medium"))
            {
                percent = 90;
            }
            else if (tyre.Name.Contains("Hard"))
            {
                percent = 75;
            }

            // Calculating TyreCoefficient
            float tyreCoefficient = tyre.DegradationCoefficient * percent / 100;

            // Calculating PointTyreDeg
            float pointTyreDeg = (trackDegPoint * temp) / tyreCoefficient;

            return pointTyreDeg;
        }

        // Method to calculate the average of the values from the PointTyreDeg formula
        private float calculateAve(float[] values)
        {
            // Average = Sum of the values / Total number of values
            float sum = values.Sum();
            return sum / values.Length;
        }

        // Method to calculate the range of values from the PointTyreDeg formula
        private float calculateRange(float[] values)
        {
            // Range = Max value - Min value
            float range = values.Max() - values.Min();
            return range;
        }

        // Method to fill results textboxes with values from calucations()
        private void fillTextboxes(float FLaverage, float FRaverage, float RLaverage, float RRaverage, float FLrange, float FRrange, float RLrange, float RRrange)
        {
            this.Dispatcher.Invoke(() =>
            {
                FLAveTextbox.Text = FLaverage.ToString();
                FLRangeTextbox.Text = FLrange.ToString();

                FRAveTextbox.Text = FRaverage.ToString();
                FRRangeTextbox.Text = FRrange.ToString();

                RLAveTextbox.Text = RLaverage.ToString();
                RLRangeTextbox.Text = RLrange.ToString();

                RRAveTextbox.Text = RRaverage.ToString();
                RRRangeTextbox.Text = RRrange.ToString();
            });
                
        }

        // Method to colour results textboxes based on value
        private void colourTextboxes(float FLaverage, float FRaverage, float RLaverage, float RRaverage, float FLrange, float FRrange, float RLrange, float RRrange)
        {
            var bc = new BrushConverter();

            this.Dispatcher.Invoke(() =>
            {
                // If statements comparing the values of the textboxes
                // All average textboxes
                if (FLaverage < 1000)
                {
                    FLAveTextbox.Background = (Brush)bc.ConvertFrom("#92D050");
                }
                else if (FLaverage >= 1000 || FLaverage <= 2999)
                {
                    FLAveTextbox.Background = (Brush)bc.ConvertFrom("#FFFF00");
                }
                else if (FLaverage >= 3000)
                {
                    FLAveTextbox.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                }

                if (FRaverage < 1000)
                {
                    FRAveTextbox.Background = (Brush)bc.ConvertFrom("#92D050");
                }
                else if (FRaverage >= 1000 || FRaverage <= 2999)
                {
                    FRAveTextbox.Background = (Brush)bc.ConvertFrom("#FFFF00");
                }
                else if (FRaverage >= 3000)
                {
                    FRAveTextbox.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                }

                if (RLaverage < 1000)
                {
                    RLAveTextbox.Background = (Brush)bc.ConvertFrom("#92D050");
                }
                else if (RLaverage >= 1000 || RLaverage <= 2999)
                {
                    RLAveTextbox.Background = (Brush)bc.ConvertFrom("#FFFF00");
                }
                else if (RLaverage >= 3000)
                {
                    RLAveTextbox.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                }

                if (RRaverage < 1000)
                {
                    RRAveTextbox.Background = (Brush)bc.ConvertFrom("#92D050");
                }
                else if (RRaverage >= 1000 || RRaverage <= 2999)
                {
                    RRAveTextbox.Background = (Brush)bc.ConvertFrom("#FFFF00");
                }
                else if (RRaverage >= 3000)
                {
                    RRAveTextbox.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                }

                // All range textboxes
                if (FLrange < 1000)
                {
                    FLRangeTextbox.Background = (Brush)bc.ConvertFrom("#92D050");
                }
                else if (FLrange >= 1000 || FLrange <= 2999)
                {
                    FLRangeTextbox.Background = (Brush)bc.ConvertFrom("#FFFF00");
                }
                else if (FLrange >= 3000)
                {
                    FLRangeTextbox.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                }

                if (FRrange < 1000)
                {
                    FRRangeTextbox.Background = (Brush)bc.ConvertFrom("#92D050");
                }
                else if (FRrange >= 1000 || FRrange <= 2999)
                {
                    FRRangeTextbox.Background = (Brush)bc.ConvertFrom("#FFFF00");
                }
                else if (FRrange >= 3000)
                {
                    FRRangeTextbox.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                }

                if (RLrange < 1000)
                {
                    RLRangeTextbox.Background = (Brush)bc.ConvertFrom("#92D050");
                }
                else if (RLrange >= 1000 || RLrange <= 2999)
                {
                    RLRangeTextbox.Background = (Brush)bc.ConvertFrom("#FFFF00");
                }
                else if (RLrange >= 3000)
                {
                    RLRangeTextbox.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                }

                if (RRrange < 1000)
                {
                    RRRangeTextbox.Background = (Brush)bc.ConvertFrom("#92D050");
                }
                else if (RRrange >= 1000 || RRrange <= 2999)
                {
                    RRRangeTextbox.Background = (Brush)bc.ConvertFrom("#FFFF00");
                }
                else if (RRrange >= 3000)
                {
                    RRRangeTextbox.Background = (Brush)bc.ConvertFrom("#FFFF0000");
                }
            });
        }

        // Method to clear all values from textboxes
        private void clearTextboxes()
        {
            this.Dispatcher.Invoke(() =>
            {
                FLAveTextbox.Text = "";
                FLRangeTextbox.Text = "";

                FRAveTextbox.Text = "";
                FRRangeTextbox.Text = "";

                RLAveTextbox.Text = "";
                RLRangeTextbox.Text = "";

                RRAveTextbox.Text = "";
                RRRangeTextbox.Text = "";
            });
        }

        void tyreValidation()
        {
            // To-Do
            // Planned future feature
        }
    }
}