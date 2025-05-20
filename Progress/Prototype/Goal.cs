using System;

namespace TelegramBot.Progress.Prototype
{
    // Основной класс Goal с расширенным набором атрибутов
    public class Goal : IPrototype<Goal>
    {
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.Now;
        public bool IsCompleted { get; set; } = false;
        public string Category { get; set; } = "Общее";
        public int Priority { get; set; } = 1; // Приоритет (1 - низкий, 5 - высокий)
        public string Notes { get; set; } = string.Empty; // Заметки
        public TimeSpan EstimatedTime { get; set; } = TimeSpan.Zero; // Оценочное время выполнения
        public DateTime? Deadline { get; set; } = null; // Крайний срок (опционально)
        public string AssignedTo { get; set; } = "Не назначено"; // Кому назначена цель

        // Переопределение ToString для отображения всех атрибутов
        public override string ToString() =>
            $"{Description} | Дата: {Date:dd.MM.yyyy} | Категория: {Category} | " +
            $"Приоритет: {Priority} | Заметки: {Notes} | " +
            $"Время: {EstimatedTime.TotalHours:F1} ч. | " +
            $"Дедлайн: {(Deadline.HasValue ? Deadline.Value.ToString("dd.MM.yyyy") : "Нет")} | " +
            $"Назначено: {AssignedTo} | {(IsCompleted ? "Выполнено ✅" : "Не выполнено ❌")}";

        // Реализация ICloneable
        public Goal Clone()
        {
            return new Goal
            {
                Description = Description,
                Date = Date,
                IsCompleted = IsCompleted,
                Category = Category,
                Priority = Priority,
                Notes = Notes,
                EstimatedTime = EstimatedTime,
                Deadline = Deadline,
                AssignedTo = AssignedTo
            };
        }

        // Метод для создания модифицированного клона
        public Goal CreateModifiedClone(string newDescription, DateTime newDate, string newCategory)
        {
            var clone = Clone();
            clone.Description = newDescription;
            clone.Date = newDate;
            clone.Category = newCategory;
            clone.IsCompleted = false; // Сбрасываем статус выполнения
            return clone;
        }
    }


    // Класс GoalBuilder для реализации паттерна "Builder"

}