using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Progress.AbstractFactory.AbstractImplements
{
    public abstract class ColdDateIdeaGenerator:IDateIdeaGenerator
    {
        protected readonly Random _random = new();
        protected readonly string[] _homeIdeas = new[]
        {
        "Укутайтесь в плед и пейте горячий шоколад.",
        "Устройте вечер у камина с настольными играми.",
        "Испеките вместе имбирное печенье или пирог.",
        "Посмотрите зимние фильмы под теплым одеялом.",
        "Создайте новогодние украшения своими руками."
    };
        protected readonly string[] _outdoorIdeas = new[]
        {
        "Покатайтесь на коньках, если есть лед.",
        "Устройте фотосессию на морозном воздухе.",
        "Постройте снеговика или поиграйте в снежки.",
        "Отправьтесь на прогулку в заснеженный лес.",
        "Посетите зимний рынок с глинтвейном."
    };

        public abstract string GenerateDateIdea();

    }
}
