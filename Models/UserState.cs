using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Models
{
    public class UserState
    {
        public int Step { get; set; } = 0; // 0 - дата, 1 - время, 2 - название
        public DateTime? EventDate { get; set; }
        public string EventTime { get; set; }
        public string EventName { get; set; }
        public DateTime CurrentMonth { get; internal set; }
    }
}
