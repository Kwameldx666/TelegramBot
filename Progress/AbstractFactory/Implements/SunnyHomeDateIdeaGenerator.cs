using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Progress.AbstractFactory.AbstractImplements;

namespace TelegramBot.Progress.AbstractFactory.Implements
{
    public class SunnyHomeDateIdeaGenerator:SunnyDateIdeaGenerator
    {
        public override string GenerateDateIdea()
        {
            return _homeIdeas[_random.Next(_homeIdeas.Length)];
        }
    }
}
