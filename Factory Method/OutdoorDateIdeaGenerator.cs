using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot.Factory_Method
{
    public class OutdoorDateIdeaGenerator : IDateIdeaGenerator
    {
        private readonly Random _random = new();
        private readonly string[] _ideas = new[]
        {
            "Прогуляйтесь по парку, держась за руки, и найдите красивое место для отдыха.",
            "Устройте вечернюю прогулку по городу, любуясь огнями.",
            "Возьмите велосипеды и покатайтесь по набережной."
        };

        public string GenerateDateIdea() => _ideas[_random.Next(_ideas.Length)];
    }
}
