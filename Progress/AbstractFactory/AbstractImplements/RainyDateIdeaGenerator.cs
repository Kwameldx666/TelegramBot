using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Progress.AbstractFactory.AbstractImplements
{
    public abstract class RainyDateIdeaGenerator : IDateIdeaGenerator
    {
        protected readonly Random _random = new();
        protected readonly string[] _homeIdeas = new[]
        {
        "Устройте уютный вечер с горячим чаем, слушая капли.",
        "Посмотрите романтические фильмы под шум дождя.",
        "Соберите пазл или сыграйте в настольные игры.",
        "Устройте кулинарный вечер, готовя теплые блюда.",
        "Создайте плейлист и устройте домашнюю танцевальную вечеринку."
    };
        protected readonly string[] _outdoorIdeas = new[]
        {
        "Возьмите зонтики и прогуляйтесь под дождем.",
        "Попрыгайте по лужам в резиновых сапогах.",
        "Посетите кафе и наблюдайте за дождем через окно.",
        "Сходите в музей или галерею, укрываясь от дождя.",
        "Отправьтесь на фотопрогулку, запечатлев дождливые пейзажи."
    };

        public abstract string GenerateDateIdea();

    }
}
