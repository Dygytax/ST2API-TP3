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
using System.Text.Json;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;

namespace TP3_UI
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
        static async Task<Rootweather> getWeather(string customRequest)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri =
                    new Uri(String.Format("https://api.openweathermap.org/data/2.5/weather?{0}&units=metric&appid=f729f099b81280e7124a6719d2184417", customRequest))
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Rootweather myDeserializedClass = JsonConvert.DeserializeObject<Rootweather>(body);
                return myDeserializedClass;
            }
        }
        static async Task<RootPollution> getPollution(string customPollution)
        {
            var client = new HttpClient();
            var requestPollution = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri =
                    new Uri(String.Format("http://api.openweathermap.org/data/2.5/air_pollution?{0}&appid=97fb1921c87c6495fb6a246da2cadc11", customPollution))
            };
            using (var responsePollution = await client.SendAsync(requestPollution))
            {
                responsePollution.EnsureSuccessStatusCode();
                var body = await responsePollution.Content.ReadAsStringAsync();
                RootPollution myPollutionClass = JsonConvert.DeserializeObject<RootPollution>(body);
                return myPollutionClass;
            }
        }
        static async Task<RootForecast> getForecast(string customForecast)
        {
            var client = new HttpClient();
            var requestForecast = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri =
                    new Uri(String.Format("https://api.openweathermap.org/data/2.5/forecast?{0}&units=metric&appid=f729f099b81280e7124a6719d2184417", customForecast))
            };
            using (var responseForecast = await client.SendAsync(requestForecast))
            {
                responseForecast.EnsureSuccessStatusCode();
                var body = await responseForecast.Content.ReadAsStringAsync();
                RootForecast myForecastClass = JsonConvert.DeserializeObject<RootForecast>(body);
                return myForecastClass;
            }
        }
        async Task display(string WeatherRequest, string PollutionRequest, string ForecastRequest)
        {
            Rootweather weather = await getWeather(WeatherRequest);
            RootPollution pollution = await getPollution(PollutionRequest);
            RootForecast forecast = await getForecast(ForecastRequest);

            DateTime dateLondonrise = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime dateLondonset = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateLondonrise = dateLondonrise.AddSeconds(weather.sys.sunrise).ToLocalTime();
            dateLondonset = dateLondonset.AddSeconds(weather.sys.sunset).ToLocalTime();

            Icon_Weather.Source = fetchImage("https://openweathermap.org/img/w/" + weather.weather[0].icon + ".png");

            Weather_Label.Content = weather.weather[0].main;
            Temp_Label.Content = weather.main.temp + " °C";
            Sunrise_Label.Content = dateLondonrise.ToString("HH:mm");
            Sunset_Label.Content = dateLondonset.ToString("HH:mm");

            if(weather.main.temp <=0 && weather.main.temp >= -10)
            {
                Temp_Label.Foreground = new SolidColorBrush(Colors.LightBlue);
            }
            else if (weather.main.temp <= 15 && weather.main.temp > 0)
            {
                Temp_Label.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            else if (weather.main.temp <= 25 && weather.main.temp > 15)
            {
                Temp_Label.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else if (weather.main.temp <= 35 && weather.main.temp > 25)
            {
                Temp_Label.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (weather.main.temp > 35)
            {
                Temp_Label.Foreground = new SolidColorBrush(Colors.DarkRed);
            }

            DateTime dateForecastLondon = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateForecastLondon = dateForecastLondon.AddSeconds(forecast.list[0].dt).ToLocalTime();
            string dateForecastLondon2 = dateForecastLondon.Date.ToString("dd/MM/yyyy");
            string descWeatherLondon = forecast.list[0].weather[0].description;
            Date_DayOne.Content = dateForecastLondon2; 
            Temp_DayOne.Content = forecast.list[0].main.temp + "°C"; 
            Weather_Day1.Content = descWeatherLondon;
            Icon_DayOne.Source = fetchImage("https://openweathermap.org/img/w/" + forecast.list[0].weather[0].icon + ".png");

            if (forecast.list[0].main.temp <= 0 && forecast.list[0].main.temp >= -10)
            {
                Temp_DayOne.Foreground = new SolidColorBrush(Colors.LightBlue);
            }
            else if (forecast.list[0].main.temp <= 15 && forecast.list[0].main.temp > 0)
            {
                Temp_DayOne.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            else if (forecast.list[0].main.temp <= 25 && forecast.list[0].main.temp > 15)
            {
                Temp_DayOne.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else if (forecast.list[0].main.temp <= 35 && forecast.list[0].main.temp > 25)
            {
                Temp_DayOne.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (forecast.list[0].main.temp > 35)
            {
                Temp_DayOne.Foreground = new SolidColorBrush(Colors.DarkRed);
            }
            else if (forecast.list[0].main.temp < -10 && forecast.list[0].main.temp >= -60)
            {
                Temp_Day5.Foreground = new SolidColorBrush(Colors.Blue);
            }

            DateTime dateForecastLondon11 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateForecastLondon11 = dateForecastLondon11.AddSeconds(forecast.list[8].dt).ToLocalTime();
            string dateForecastLondon3 = dateForecastLondon11.Date.ToString("dd/MM/yyyy");
            string descWeatherLondon2 = forecast.list[8].weather[0].description;
            Date_Day2.Content = dateForecastLondon3;
            Temp_Day2.Content = forecast.list[8].main.temp + "°C";
            Weather_Day2.Content = descWeatherLondon2;
            Icon_Day2.Source = fetchImage("https://openweathermap.org/img/w/" + forecast.list[8].weather[0].icon + ".png");

            if (forecast.list[8].main.temp <= 0 && forecast.list[8].main.temp >= -10)
            {
                Temp_Day2.Foreground = new SolidColorBrush(Colors.LightBlue);
            }
            else if (forecast.list[8].main.temp <= 15 && forecast.list[8].main.temp > 0)
            {
                Temp_Day2.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            else if (forecast.list[8].main.temp <= 25 && forecast.list[8].main.temp > 15)
            {
                Temp_Day2.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else if (forecast.list[8].main.temp <= 35 && forecast.list[8].main.temp > 25)
            {
                Temp_Day2.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (forecast.list[8].main.temp > 35)
            {
                Temp_Day2.Foreground = new SolidColorBrush(Colors.DarkRed);
            }
            else if (forecast.list[8].main.temp < -10 && forecast.list[8].main.temp >= -60)
            {
                Temp_Day5.Foreground = new SolidColorBrush(Colors.Blue);
            }

            DateTime dateForecastLondon12 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateForecastLondon12 = dateForecastLondon12.AddSeconds(forecast.list[16].dt).ToLocalTime();
            string dateForecastLondon4 = dateForecastLondon12.Date.ToString("dd/MM/yyyy");
            string descWeatherLondon3 = forecast.list[16].weather[0].description;
            Date_Day3.Content = dateForecastLondon4;
            Temp_Day3.Content = forecast.list[16].main.temp + "°C";
            Weather_Day3.Content = descWeatherLondon3;
            Icon_Day3.Source = fetchImage("https://openweathermap.org/img/w/" + forecast.list[16].weather[0].icon + ".png");

            if (forecast.list[16].main.temp <= 0 && forecast.list[16].main.temp >= -10)
            {
                Temp_Day3.Foreground = new SolidColorBrush(Colors.LightBlue);
            }
            else if (forecast.list[16].main.temp <= 15 && forecast.list[16].main.temp > 0)
            {
                Temp_Day3.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            else if (forecast.list[16].main.temp <= 25 && forecast.list[16].main.temp > 15)
            {
                Temp_Day3.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else if (forecast.list[16].main.temp <= 35 && forecast.list[16].main.temp > 25)
            {
                Temp_Day3.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (forecast.list[16].main.temp > 35)
            {
                Temp_Day3.Foreground = new SolidColorBrush(Colors.DarkRed);
            }
            else if (forecast.list[16].main.temp < -10 && forecast.list[16].main.temp >= -60)
            {
                Temp_Day5.Foreground = new SolidColorBrush(Colors.Blue);
            }


            DateTime dateForecastLondon13 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateForecastLondon13 = dateForecastLondon13.AddSeconds(forecast.list[24].dt).ToLocalTime();
            string dateForecastLondon5 = dateForecastLondon13.Date.ToString("dd/MM/yyyy");
            string descWeatherLondon4 = forecast.list[24].weather[0].description;
            Date_Day4.Content = dateForecastLondon5;
            Temp_Day4.Content = forecast.list[24].main.temp + "°C";
            Weather_Day4.Content = descWeatherLondon4;
            Icon_Day4.Source = fetchImage("https://openweathermap.org/img/w/" + forecast.list[24].weather[0].icon + ".png");

            if (forecast.list[24].main.temp <= 0 && forecast.list[24].main.temp >= -10)
            {
                Temp_Day4.Foreground = new SolidColorBrush(Colors.LightBlue);
            }
            else if (forecast.list[24].main.temp <= 15 && forecast.list[24].main.temp > 0)
            {
                Temp_Day4.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            else if (forecast.list[24].main.temp <= 25 && forecast.list[24].main.temp > 15)
            {
                Temp_Day4.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else if (forecast.list[24].main.temp <= 35 && forecast.list[24].main.temp > 25)
            {
                Temp_Day4.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (forecast.list[24].main.temp > 35)
            {
                Temp_Day4.Foreground = new SolidColorBrush(Colors.DarkRed);
            }
            else if (forecast.list[24].main.temp < -10 && forecast.list[24].main.temp >= -60)
            {
                Temp_Day5.Foreground = new SolidColorBrush(Colors.Blue);
            }

            DateTime dateForecastLondon14 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateForecastLondon14 = dateForecastLondon14.AddSeconds(forecast.list[32].dt).ToLocalTime();
            string dateForecastLondon6 = dateForecastLondon14.Date.ToString("dd/MM/yyyy");
            string descWeatherLondon5 = forecast.list[32].weather[0].description;
            Date_Day5.Content = dateForecastLondon6;
            Temp_Day5.Content = forecast.list[32].main.temp + "°C";
            Weather_Day5.Content = descWeatherLondon5;
            Icon_Day5.Source = fetchImage("https://openweathermap.org/img/w/" + forecast.list[32].weather[0].icon + ".png");

            if (forecast.list[32].main.temp <= 0 && forecast.list[32].main.temp >= -10)
            {
                Temp_Day5.Foreground = new SolidColorBrush(Colors.LightBlue);
            }
            else if (forecast.list[32].main.temp <= 15 && forecast.list[32].main.temp > 0)
            {
                Temp_Day5.Foreground = new SolidColorBrush(Colors.Yellow);
            }
            else if (forecast.list[32].main.temp <= 25 && forecast.list[32].main.temp > 15)
            {
                Temp_Day5.Foreground = new SolidColorBrush(Colors.Orange);
            }
            else if (forecast.list[32].main.temp <= 35 && forecast.list[32].main.temp > 25)
            {
                Temp_Day5.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (forecast.list[32].main.temp > 35)
            {
                Temp_Day5.Foreground = new SolidColorBrush(Colors.DarkRed);
            }
            else if (forecast.list[32].main.temp <-10  && forecast.list[32].main.temp >= -60)
            {
                Temp_Day5.Foreground = new SolidColorBrush(Colors.Blue);
            }

            double pollutionRate = pollution.list[0].main.aqi;
            string rate = null;
            if (pollutionRate == 1)
            {
                rate = "Good";
            }
            else if (pollutionRate == 2)
            {
                rate = "Fair";
            }
            else if (pollutionRate == 3)
            {
                rate = "Moderate";
            }
            else if (pollutionRate == 4)
            {
                rate = "Poor";
            }
            else if (pollutionRate == 5)
            {
                rate = "Very Poor";
            }
            pollution_grade.Content = rate;
            pollution_rate.Content = pollutionRate;

            if (rate == "Good")
            {
                pollution_rate.Foreground = new SolidColorBrush(Colors.LightGreen);
                pollution_grade.Foreground = new SolidColorBrush(Colors.LightGreen);
            }
            else if (rate == "Fair")
            {
                pollution_rate.Foreground = new SolidColorBrush(Colors.LimeGreen);
                pollution_grade.Foreground = new SolidColorBrush(Colors.LimeGreen);
            }
            else if (rate == "Moderate")
            {
                pollution_rate.Foreground = new SolidColorBrush(Colors.DarkGreen);
                pollution_grade.Foreground = new SolidColorBrush(Colors.DarkGreen);
            }
            else if (rate == "Poor")
            {
                pollution_rate.Foreground = new SolidColorBrush(Colors.Red);
                pollution_grade.Foreground = new SolidColorBrush(Colors.Red);
            }
            else if (rate == "Very Poor")
            {
                pollution_rate.Foreground = new SolidColorBrush(Colors.DarkRed);
                pollution_grade.Foreground = new SolidColorBrush(Colors.DarkRed);
            }
        }
        public BitmapImage fetchImage(string url)
        {
            var fullFilePath = @url;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();
            return bitmap;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            display("q=London", "lat=51.5085&lon=0.1257", "q=London");
            Button_London.Background = new SolidColorBrush(Colors.LightBlue);
            Button_Paris.Background = new SolidColorBrush(Colors.LightGray);
            Button_NewYork.Background = new SolidColorBrush(Colors.LightGray);
            Button_Moscow.Background = new SolidColorBrush(Colors.LightGray);
            Button_Dubai.Background = new SolidColorBrush(Colors.LightGray);
            Button_Tokyo.Background = new SolidColorBrush(Colors.LightGray);
            Button_Singapore.Background = new SolidColorBrush(Colors.LightGray);
            Button_LosAngeles.Background = new SolidColorBrush(Colors.LightGray);
            Button_Barcelona.Background = new SolidColorBrush(Colors.LightGray);
            Button_Madrid.Background = new SolidColorBrush(Colors.LightGray);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            display("q=Paris", "lat=48.8534&lon=2.3488", "q=Paris");
            Button_London.Background = new SolidColorBrush(Colors.LightGray);
            Button_Paris.Background = new SolidColorBrush(Colors.LightBlue);
            Button_NewYork.Background = new SolidColorBrush(Colors.LightGray);
            Button_Moscow.Background = new SolidColorBrush(Colors.LightGray);
            Button_Dubai.Background = new SolidColorBrush(Colors.LightGray);
            Button_Tokyo.Background = new SolidColorBrush(Colors.LightGray);
            Button_Singapore.Background = new SolidColorBrush(Colors.LightGray);
            Button_LosAngeles.Background = new SolidColorBrush(Colors.LightGray);
            Button_Barcelona.Background = new SolidColorBrush(Colors.LightGray);
            Button_Madrid.Background = new SolidColorBrush(Colors.LightGray);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            display("q=New York", "lat=40.7143&lon=-74.006", "q=New York");
            Button_London.Background = new SolidColorBrush(Colors.LightGray);
            Button_Paris.Background = new SolidColorBrush(Colors.LightGray);
            Button_NewYork.Background = new SolidColorBrush(Colors.LightBlue);
            Button_Moscow.Background = new SolidColorBrush(Colors.LightGray);
            Button_Dubai.Background = new SolidColorBrush(Colors.LightGray);
            Button_Tokyo.Background = new SolidColorBrush(Colors.LightGray);
            Button_Singapore.Background = new SolidColorBrush(Colors.LightGray);
            Button_LosAngeles.Background = new SolidColorBrush(Colors.LightGray);
            Button_Barcelona.Background = new SolidColorBrush(Colors.LightGray);
            Button_Madrid.Background = new SolidColorBrush(Colors.LightGray);
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            display("q=Moscow", "lat=55.7522&lon=37.6156", "q=Moscow");
            Button_London.Background = new SolidColorBrush(Colors.LightGray);
            Button_Paris.Background = new SolidColorBrush(Colors.LightGray);
            Button_NewYork.Background = new SolidColorBrush(Colors.LightGray);
            Button_Moscow.Background = new SolidColorBrush(Colors.LightBlue);
            Button_Dubai.Background = new SolidColorBrush(Colors.LightGray);
            Button_Tokyo.Background = new SolidColorBrush(Colors.LightGray);
            Button_Singapore.Background = new SolidColorBrush(Colors.LightGray);
            Button_LosAngeles.Background = new SolidColorBrush(Colors.LightGray);
            Button_Barcelona.Background = new SolidColorBrush(Colors.LightGray);
            Button_Madrid.Background = new SolidColorBrush(Colors.LightGray);
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            display("q=Dubai", "lat=25.2582&lon=55.3047", "q=Dubai");
            Button_London.Background = new SolidColorBrush(Colors.LightGray);
            Button_Paris.Background = new SolidColorBrush(Colors.LightGray);
            Button_NewYork.Background = new SolidColorBrush(Colors.LightGray);
            Button_Moscow.Background = new SolidColorBrush(Colors.LightGray);
            Button_Dubai.Background = new SolidColorBrush(Colors.LightBlue);
            Button_Tokyo.Background = new SolidColorBrush(Colors.LightGray);
            Button_Singapore.Background = new SolidColorBrush(Colors.LightGray);
            Button_LosAngeles.Background = new SolidColorBrush(Colors.LightGray);
            Button_Barcelona.Background = new SolidColorBrush(Colors.LightGray);
            Button_Madrid.Background = new SolidColorBrush(Colors.LightGray);
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            display("q=Tokyo", "lat=35.6895&lon=139.6917", "q=Tokyo");
            Button_London.Background = new SolidColorBrush(Colors.LightGray);
            Button_Paris.Background = new SolidColorBrush(Colors.LightGray);
            Button_NewYork.Background = new SolidColorBrush(Colors.LightGray);
            Button_Moscow.Background = new SolidColorBrush(Colors.LightGray);
            Button_Dubai.Background = new SolidColorBrush(Colors.LightGray);
            Button_Tokyo.Background = new SolidColorBrush(Colors.LightBlue);
            Button_Singapore.Background = new SolidColorBrush(Colors.LightGray);
            Button_LosAngeles.Background = new SolidColorBrush(Colors.LightGray);
            Button_Barcelona.Background = new SolidColorBrush(Colors.LightGray);
            Button_Madrid.Background = new SolidColorBrush(Colors.LightGray);
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            display("q=Singapore", "lat=1.2897&lon=103.8501", "q=Singapore");
            Button_London.Background = new SolidColorBrush(Colors.LightGray);
            Button_Paris.Background = new SolidColorBrush(Colors.LightGray);
            Button_NewYork.Background = new SolidColorBrush(Colors.LightGray);
            Button_Moscow.Background = new SolidColorBrush(Colors.LightGray);
            Button_Dubai.Background = new SolidColorBrush(Colors.LightGray);
            Button_Tokyo.Background = new SolidColorBrush(Colors.LightGray);
            Button_Singapore.Background = new SolidColorBrush(Colors.LightBlue);
            Button_LosAngeles.Background = new SolidColorBrush(Colors.LightGray);
            Button_Barcelona.Background = new SolidColorBrush(Colors.LightGray);
            Button_Madrid.Background = new SolidColorBrush(Colors.LightGray);
        }
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            display("q=Los Angeles", "lat=34.0522&lon=-118.2437", "q=Los Angeles");
            Button_London.Background = new SolidColorBrush(Colors.LightGray);
            Button_Paris.Background = new SolidColorBrush(Colors.LightGray);
            Button_NewYork.Background = new SolidColorBrush(Colors.LightGray);
            Button_Moscow.Background = new SolidColorBrush(Colors.LightGray);
            Button_Dubai.Background = new SolidColorBrush(Colors.LightGray);
            Button_Tokyo.Background = new SolidColorBrush(Colors.LightGray);
            Button_Singapore.Background = new SolidColorBrush(Colors.LightGray);
            Button_LosAngeles.Background = new SolidColorBrush(Colors.LightBlue);
            Button_Barcelona.Background = new SolidColorBrush(Colors.LightGray);
            Button_Madrid.Background = new SolidColorBrush(Colors.LightGray);
        }
        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            display("q=Barcelona", "lat=41.3888&lon=2.159", "q=Barcelona");
            Button_London.Background = new SolidColorBrush(Colors.LightGray);
            Button_Paris.Background = new SolidColorBrush(Colors.LightGray);
            Button_NewYork.Background = new SolidColorBrush(Colors.LightGray);
            Button_Moscow.Background = new SolidColorBrush(Colors.LightGray);
            Button_Dubai.Background = new SolidColorBrush(Colors.LightGray);
            Button_Tokyo.Background = new SolidColorBrush(Colors.LightGray);
            Button_Singapore.Background = new SolidColorBrush(Colors.LightGray);
            Button_LosAngeles.Background = new SolidColorBrush(Colors.LightGray);
            Button_Barcelona.Background = new SolidColorBrush(Colors.LightBlue);
            Button_Madrid.Background = new SolidColorBrush(Colors.LightGray);
        }
        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            display("q=Madrid", "lat=40.4165&lon=-3.7026", "q=Madrid");
            Button_London.Background = new SolidColorBrush(Colors.LightGray);
            Button_Paris.Background = new SolidColorBrush(Colors.LightGray);
            Button_NewYork.Background = new SolidColorBrush(Colors.LightGray);
            Button_Moscow.Background = new SolidColorBrush(Colors.LightGray);
            Button_Dubai.Background = new SolidColorBrush(Colors.LightGray);
            Button_Tokyo.Background = new SolidColorBrush(Colors.LightGray);
            Button_Singapore.Background = new SolidColorBrush(Colors.LightGray);
            Button_LosAngeles.Background = new SolidColorBrush(Colors.LightGray);
            Button_Barcelona.Background = new SolidColorBrush(Colors.LightGray);
            Button_Madrid.Background = new SolidColorBrush(Colors.LightBlue);
        }
    }
}
