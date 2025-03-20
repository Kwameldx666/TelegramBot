using Telegram.Bot;
using TelegramBot.Factory_Method;
using TelegramBot.Models;
using TelegramBot.Observer.Intefaces;
using TelegramBot.Strategy;

public class EventTimer : ISubject, IStrategy
{
    private List<IObserver> _observers = new List<IObserver>();
    private DateTime _targetTime;
    private string? _eventName;
    private bool _hasNotified = false;

    public void Attach(IObserver observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void Detach(IObserver observer) => _observers.Remove(observer);

    public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, UserState state, string input, CancellationToken cancellationToken)
    {
        if (state.EventDate.HasValue && state.EventTime != null && state.EventName != null)
        {
            _targetTime = state.EventDate.Value + TimeSpan.Parse(state.EventTime);
            _eventName = state.EventName;
            _hasNotified = false;

            await botClient.SendTextMessageAsync(chatId,
                $"Таймер для '{_eventName}' на {_targetTime:dd.MM.yyyy HH:mm} установлен!",
                replyMarkup: Program.GetStartMenu(),
                cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(chatId,
                "Сначала настройте событие через 'Таймер любви ⏳'!",
                replyMarkup: Program.GetStartMenu(),
                cancellationToken: cancellationToken);
        }
    }

    public void StartTimer(ITelegramBotClient botClient)
    {
        var timeUntilEvent = _targetTime - DateTime.Now;
        if (timeUntilEvent.TotalMilliseconds > 0)
        {
            var timer = new Timer(async state => await NotifyEventTime(botClient), null, (int)timeUntilEvent.TotalMilliseconds, Timeout.Infinite);
        }
        else
        {
            Task.Run(() => NotifyEventTime(botClient));
        }
    }

    private async Task NotifyEventTime(ITelegramBotClient botClient)
    {
        if (!_hasNotified)
        {
            _hasNotified = true;
            foreach (var observer in _observers.ToList())
            {
                await observer.Update($"Время пришло! {_eventName} начинается прямо сейчас!");
                Detach(observer);
            }
        }
    }

    private string GetRemainingTimeMessage()
    {
        var timeLeft = _targetTime - DateTime.Now;
        if (timeLeft.TotalSeconds <= 0)
            return $"Событие '{_eventName}' уже наступило или прошло {Math.Abs(timeLeft.TotalDays)} дней назад!";
        if (timeLeft.TotalDays > 1)
            return $"До '{_eventName}' осталось {(int)timeLeft.TotalDays} дней, {(int)timeLeft.Hours} часов!";
        else if (timeLeft.TotalHours > 1)
            return $"До '{_eventName}' осталось {(int)timeLeft.TotalHours} часов, {(int)timeLeft.Minutes} минут!";
        else
            return $"До '{_eventName}' осталось {(int)timeLeft.TotalMinutes} минут! Готовься!";
    }

    public async Task Notify()
    {
        if (_eventName == null)
        {
            foreach (var observer in _observers)
            {
                await observer.Update("Таймер не установлен!");
            }
        }
        else
        {
            foreach (var observer in _observers)
            {
                await observer.Update(GetRemainingTimeMessage());
            }
        }
    }
}
