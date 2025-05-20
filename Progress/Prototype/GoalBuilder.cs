using TelegramBot.Progress.Prototype;

public class GoalBuilder
{
    private Goal _goal;

    // Конструктор создает объект Goal с значениями по умолчанию
    public GoalBuilder()
    {
        _goal = new Goal();
    }

    // Методы для пошагового задания свойств
    public GoalBuilder SetDescription(string description)
    {
        _goal.Description = description;
        return this; // Возвращаем this для цепочки вызовов
    }

    public GoalBuilder SetDate(DateTime date)
    {
        _goal.Date = date;
        return this;
    }

    public GoalBuilder SetCompleted(bool isCompleted)
    {
        _goal.IsCompleted = isCompleted;
        return this;
    }

    public GoalBuilder SetCategory(string category)
    {
        _goal.Category = category;
        return this;
    }

    public GoalBuilder SetPriority(int priority)
    {
        if (priority < 1 || priority > 5)
            throw new ArgumentException("Приоритет должен быть в диапазоне от 1 до 5");
        _goal.Priority = priority;
        return this;
    }

    public GoalBuilder SetNotes(string notes)
    {
        _goal.Notes = notes;
        return this;
    }

    public GoalBuilder SetEstimatedTime(TimeSpan estimatedTime)
    {
        _goal.EstimatedTime = estimatedTime;
        return this;
    }

    public GoalBuilder SetDeadline(DateTime? deadline)
    {
        _goal.Deadline = deadline;
        return this;
    }

    public GoalBuilder SetAssignedTo(string assignedTo)
    {
        _goal.AssignedTo = assignedTo;
        return this;
    }

    // Завершающий метод, возвращающий готовый объект
    public Goal Build()
    {
        return _goal;
    }

    // Метод для создания объекта и сброса строителя
    public Goal BuildAndReset()
    {
        Goal result = _goal;
        _goal = new Goal(); // Сбрасываем для следующего использования
        return result;
    }
}