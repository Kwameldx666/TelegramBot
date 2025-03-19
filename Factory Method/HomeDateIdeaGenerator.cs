using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Factory_Method
{
    public class HomeDateIdeaGenerator : IDateIdeaGenerator
    {
        private readonly Random _random = new();
        private readonly string[] _ideas = new[]
        {
            "Расстелите плед на полу и устройте домашний пикник с вином и фруктами.",
            "Устройте вечер кино с попкорном и уютным одеялом.",
            "Сыграйте в настольную игру при свете лампы."
        };

        public string GenerateDateIdea() => _ideas[_random.Next(_ideas.Length)];
    }
}
