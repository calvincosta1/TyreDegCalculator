namespace TyreDegCalculator.Models
{
    class WeatherModel
    {
        public class main
        {
            // Properties of the weather
            public float temp { get; set; }
        }

        public class root
        {
            public main main { get; set; }
        }
    }
}