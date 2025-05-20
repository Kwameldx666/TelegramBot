using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TelegramBot.Factory_Method;
using TelegramBot.Progress.AbstractFactory.AbstractImplements;

namespace TelegramBot.Progress.Decorator
{
    // Декоратор для предсказания погоды
    public class WeatherPredictionDecorator : AbstractWeatherDecorator
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "bd5e378503939ddaee76f12ad7a97608"; 
        private readonly string _city;
        public WeatherPredictionDecorator(IDateIdeaGenerator baseGenerator, string city)
                : base(baseGenerator)
        {
            _httpClient = new HttpClient();
            _city = city;
        }


        public override async Task<string> GenerateDateIdeaAsync()
        {
            string baseIdea = _baseGenerator.GenerateDateIdea();
            string weather = await GetWeatherAsync();
            return $"{baseIdea} {AdjustForWeather(weather)}";
        }

        public override string GenerateDateIdea()
        {
            return GenerateDateIdeaAsync().GetAwaiter().GetResult();
        }

        public override async Task<string> GetWeatherAsync()
        {
            try
            {
                string url = $"http://api.openweathermap.org/data/2.5/weather?q={_city}&appid={_apiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(json);
                JsonElement root = doc.RootElement;
                JsonElement weatherElement = root.GetProperty("weather")[0];
                string mainWeather = weatherElement.GetProperty("main").GetString().ToLower();

                // Упрощённая классификация погоды
                if (mainWeather.Contains("clear") || mainWeather.Contains("sun"))
                    return "warm";
                if (mainWeather.Contains("rain") || mainWeather.Contains("drizzle"))
                    return "rain";
                if (mainWeather.Contains("snow") || mainWeather.Contains("cold") || root.GetProperty("main").GetProperty("temp").GetDouble() < 5)
                    return "cold";
                return "warm"; // По умолчанию
            }
            catch (Exception)
            {
                return "unknown"; // Если запрос не удался
            }
        }

        protected override string AdjustForWeather(string weather)
        {
            return weather switch
            {
                "warm" => "Погода тёплая и солнечная!",
                "cold" => "На улице холодно.",
                "rain" => "Дождь за окном.",
                _ => "Погода неизвестна — будьте готовы ко всему!"
            };
        }
    }
}
