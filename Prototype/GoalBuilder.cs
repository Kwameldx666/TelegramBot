using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Prototype
{
    public class GoalBuilder : IGoalBuilder
    {
        private Goal _goal = new();

        public IGoalBuilder SetDescription(string description)
        {
            _goal.Description = description;
            return this;
        }

        public IGoalBuilder SetDate(DateTime date)
        {
            _goal.Date = date;
            return this;
        }

        public IGoalBuilder SetCategory(string category)
        {
            _goal.Category = category;
            return this;
        }

        public IGoalBuilder SetCompleted(bool isCompleted)
        {
            _goal.IsCompleted = isCompleted;
            return this;
        }

        public Goal Build()
        {
            var result = _goal;
            _goal = new Goal();
            return result;
        }
    }
}
