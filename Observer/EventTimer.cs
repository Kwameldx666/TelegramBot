using Telegram.Bot;
using TelegramBot.Models;
using TelegramBot.Observer.Intefaces;
using TelegramBot.Observer;
using TelegramBot.Strategy;

public class EventTimer : ISubject, IStrategy
{
    private List<IObserver> _observers = new List<IObserver>();
    private DateTime _targetTime;
    private string? _eventName;
    private Timer? _timer;
    private int _lastNotifiedDays = -1;

    public void Attach(IObserver observer) => _observers.Add(observer);
    public void Detach(IObserver observer) => _observers.Remove(observer);

    public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, UserState state, string input, CancellationToken cancellationToken)
    {
        if (state.EventDate.HasValue && state.EventTime != null && state.EventName != null)
        {
            _targetTime = state.EventDate.Value + TimeSpan.Parse(state.EventTime);
            _eventName = state.EventName;

            var observer = new ConcreteObserver(chatId, botClient);
            Attach(observer);

            _timer = new Timer(CheckTime, null, 0, 60000);

            await botClient.SendTextMessageAsync(chatId,
                $"Таймер для '{_eventName}' на {_targetTime:dd.MM.yyyy HH:mm} установлен!",
                cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(chatId,
                "Сначала настройте событие через 'Таймер любви ⏳'!",
                cancellationToken: cancellationToken);
        }
    }

    private void CheckTime(object? state)
    {
        var remainingTime = _targetTime - DateTime.Now;
        var remainingDays = (int)remainingTime.TotalDays;

        if (remainingTime.TotalSeconds > 0)
        {
            if (_lastNotifiedDays != remainingDays)
            {
                _lastNotifiedDays = remainingDays;
                Notify();
            }
        }
        else
        {
            foreach (var observer in _observers)
                observer.Update($"Время пришло! {_eventName} начинается прямо сейчас!");
            _timer?.Dispose();
            _observers.Clear();
        }
    }

    private string GetRemainingTimeMessage()
    {
        var timeLeft = _targetTime - DateTime.Now;
        if (timeLeft.TotalDays > 1)
            return $"До {_eventName} осталось {(int)timeLeft.TotalDays} дней, {(int)timeLeft.Hours} часов!";
        else if (timeLeft.TotalHours > 1)
            return $"До {_eventName} осталось {(int)timeLeft.TotalHours} часов, {(int)timeLeft.Minutes} минут!";
        else
            return $"До {_eventName} осталось {(int)timeLeft.TotalMinutes} минут! Готовься!";
    }

    public async Task Notify()
    {
        Console.WriteLine($"Chat: Вызван Notify, количество наблюдателей: {_observers.Count}");
        foreach (var observer in _observers)
        {
            await observer.Update(GetRemainingTimeMessage());
        }
    }
}