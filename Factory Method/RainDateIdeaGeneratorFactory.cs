using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Factory_Method
{
    public class RainDateIdeaGeneratorFactory : IDateIdeaGeneratorFactory
    {
        public IDateIdeaGenerator CreateGenerator()
        {
            return new RainDateIdeaGenerator();
        }

        private class RainDateIdeaGenerator : IDateIdeaGenerator
        {
            private readonly Random _random = new();
            private readonly string[] _ideas = new[]
            {
                "Останьтесь дома, поставьте кастрюлю с водой на подоконник, чтобы стук дождя смешивался с её звоном, и сыграйте в настольную игру при свете лампы.",
                "Возьмите зонтики, наденьте резиновые сапоги и отправьтесь прыгать по лужам, а после вернитесь домой за горячим чаем с мёдом.",
                "Устройте дома кинотеатр: постройте 'шалаш' из одеял, включите романтический фильм и слушайте дождь за окном с попкорном в руках."
            };

            public string GenerateDateIdea()
            {
                return _ideas[_random.Next(_ideas.Length)];
            }
        }
    }
}
