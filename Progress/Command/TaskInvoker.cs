using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramBot.Progress.Command
{
    public class TaskInvoker
    {
        private readonly ICommand[] _commands;
        private readonly TimeSpan _delayBetweenTasks;

        public TaskInvoker(ICommand[] commands, TimeSpan? delayBetweenTasks = null)
        {
            _commands = commands ?? throw new ArgumentNullException(nameof(commands));
            if (_commands.Length < 2)
                throw new ArgumentException("At least two commands are required.", nameof(commands));
            _delayBetweenTasks = delayBetweenTasks ?? TimeSpan.FromSeconds(5); // Задержка по умолчанию 5 секунд
        }
        public async Task ExecuteCommand(long chatId, CancellationToken cancellationToken)
        {
            // Выбираем два случайных таска
            var random = new Random();
            var selectedCommands = _commands.OrderBy(_ => random.Next()).Take(2).ToArray();

            // Выполняем таски с задержкой
            for (int i = 0; i < selectedCommands.Length; i++)
            {
                try
                {
                    await selectedCommands[i].Execute(chatId, cancellationToken);
                    if (i < selectedCommands.Length - 1) // Задержка только между задачами
                    {
                        await Task.Delay(_delayBetweenTasks, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    // Логируем ошибку (в реальном приложении можно использовать логгер)
                    Console.WriteLine($"Error executing command {i + 1}: {ex.Message}");
                    throw; // Можно убрать throw, если не хотите прерывать выполнение
                }
            }
        }
    }
}
