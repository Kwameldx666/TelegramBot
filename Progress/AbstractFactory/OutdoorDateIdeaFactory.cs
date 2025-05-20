using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TelegramBot.Progress.AbstractFactory.AbstractImplements;
using TelegramBot.Progress.AbstractFactory.Implements;
using TelegramBot.Progress.Decorator;

namespace TelegramBot.Progress.AbstractFactory
{
    // Фабрика для улицы
    public class OutdoorDateIdeaFactory : IDateIdeaFactory
    {
        private readonly string _city;


        public OutdoorDateIdeaFactory(string city)
        {
            _city = city;
        }

        public IDateIdeaGenerator CreateSunnyDateIdeaGenerator()
        {
            return new WeatherPredictionDecorator(new SunnyHomeDateIdeaGenerator(), _city);
        }

        public IDateIdeaGenerator CreateRainyDateIdeaGenerator()
        {
            return new WeatherPredictionDecorator(new RainyHomeDateIdeaGenerator(), _city);
        }

        public IDateIdeaGenerator CreateColdDateIdeaGenerator()
        {
            return new WeatherPredictionDecorator(new ColdHomeDateIdeaGenerator(), _city);
        }
    }
}
