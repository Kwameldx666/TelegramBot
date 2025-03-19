using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Decorator;

namespace TelegramBot.Factory_Method
{
    public class OutdoorDateIdeaFactory : IDateIdeaGeneratorFactory
    {
        private readonly string _city;

        public OutdoorDateIdeaFactory(string city)
        {
            _city = city;
        }

        public IDateIdeaGenerator CreateGenerator()
        {
            return new WeatherPredictionDecorator(new OutdoorDateIdeaGenerator(), _city);
        }
    }
}
