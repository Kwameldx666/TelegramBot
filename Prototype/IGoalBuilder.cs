using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Prototype
{
    public interface IGoalBuilder
    {
        IGoalBuilder SetDescription(string description);
        IGoalBuilder SetDate(DateTime date);
        IGoalBuilder SetCategory(string category);
        IGoalBuilder SetCompleted(bool isCompleted);
        Goal Build();
    }
}
