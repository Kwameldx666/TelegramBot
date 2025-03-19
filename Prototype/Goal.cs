using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Prototype
{
    public class Goal : ICloneable
    {
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Category { get; set; } = "Общее";

        public override string ToString() =>
            $"{Description} | Дата: {Date:dd.MM.yyyy} | Категория: {Category} | {(IsCompleted ? "Выполнено ✅" : "Не выполнено ❌")}";

        public object Clone()
        {
            return new Goal
            {
                Description = Description,
                Date = Date,
                IsCompleted = IsCompleted,
                Category = Category
            };
        }

        public Goal CreateModifiedClone(string newDescription, DateTime newDate, string newCategory)
        {
            var clone = (Goal)Clone();
            clone.Description = newDescription;
            clone.Date = newDate;
            clone.Category = newCategory;
            clone.IsCompleted = false;
            return clone;
        }
    }
}
