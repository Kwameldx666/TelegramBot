using System.Threading.Tasks;
using TelegramBot.Progress.AbstractFactory.AbstractImplements;

namespace TelegramBot.Progress.Decorator
{
    // Абстрактный декоратор
    public abstract class AbstractWeatherDecorator : IDateIdeaGenerator
    {
        protected readonly IDateIdeaGenerator _baseGenerator;

        protected AbstractWeatherDecorator(IDateIdeaGenerator baseGenerator)
        {
            _baseGenerator = baseGenerator;
        }

        // Реализация интерфейса: асинхронный метод
        public virtual async Task<string> GenerateDateIdeaAsync()
        {
            string baseIdea = _baseGenerator.GenerateDateIdea();
            return $"{baseIdea}";
        }

        // Реализация интерфейса: синхронный метод
        public virtual string GenerateDateIdea()
        {
            return _baseGenerator.GenerateDateIdea();
        }

        // Абстрактный метод для получения погодной информации
        public abstract Task<string> GetWeatherAsync();

        // Абстрактный метод для корректировки идеи в зависимости от погоды
        protected abstract string AdjustForWeather(string weather);
    }
}