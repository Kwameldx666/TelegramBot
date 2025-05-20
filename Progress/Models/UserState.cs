using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Progress.Prototype;

namespace TelegramBot.Progress.Models
{
    public class UserState
    {
        public int Step { get; set; } = 0; // 0 - дата, 1 - время, 2 - название
        public DateTime? EventDate { get; set; }
        public string EventTime { get; set; }
        public bool IsChatStarted { get; set; }
        public string EventName { get; set; }
        public DateTime CurrentMonth { get; internal set; }
        public List<string> QuizAnswers { get; set; } 
        public int QuizQuestionIndex { get; set; } = 0;
        public string? DateIdeaLocation { get; set; }
        public string? DateIdeaWeather { get; set; }
        public string? DateIdeaCity { get; set; }
        public GoalBuilder? CurrentGoalBuilder { get; set; }
        public List<Goal> Goals { get; set; } = new List<Goal>();
        public int GoalStep { get; set; } = 0;
        public string TempCategory { get; set; }
        public DateTime TempGoalDate { get; set; }

        public DateTime CurrentGoalMonth { get; set; } = DateTime.Today;
        public DateTime? CloneSourceDate { get; set; }
        public string? TempGoalTime { get; internal set; }

        public void Reset()
        {
            Step = 0;
            EventDate = null;
            EventTime = null;
            EventName = null;
            CurrentMonth = DateTime.Today;
            IsChatStarted = false;
            DateIdeaLocation = null;
            DateIdeaCity = null;
            DateIdeaWeather = null;
            Goals.Clear();
            GoalStep = 0;
            TempGoalDate = DateTime.MinValue;
            TempCategory = null;
            CloneSourceDate = null;
        }
    }
}
