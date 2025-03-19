using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Decorator;

namespace TelegramBot.Factory_Method
{
    public class HomeDateIdeaFactory : IDateIdeaGeneratorFactory
    {
        private readonly string _city;

        public HomeDateIdeaFactory(string city)
        {
            _city = city;
        }

        public IDateIdeaGenerator CreateGenerator()
        {
            return new WeatherPredictionDecorator(new HomeDateIdeaGenerator(), _city);
        }
    }
}
