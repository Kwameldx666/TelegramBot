using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Progress.AbstractFactory.AbstractImplements;

namespace TelegramBot.Progress.AbstractFactory.Implements
{
    public class ColdOutdoorDateIdeaGenerator:ColdDateIdeaGenerator
    {
        public override string GenerateDateIdea()
        {
            return _outdoorIdeas[_random.Next(_outdoorIdeas.Length)];
        }
    }
}
