using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Observer.Intefaces
{
    public interface IObserver
    {
        Task Update(string message);
    }
}
