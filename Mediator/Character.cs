using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBot.Mediator
{
    // Класс персонажа с интеграцией ИИ
    public class Character : IDisposable
    {
        private string _mood;
        private readonly Random _random = new();
        private int _interactionCount;
        private readonly HttpClient _httpClient;
        private const string ApiKey = "sk-8Psd8EtUtkatcaZL2yun9BcMlpU1F5e6yisBVVupz5JczEQS";
        private const string BaseUrl = "https://api.gptgod.online/v1/";

        public Character()
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {ApiKey}");
        }


        public async Task<string> Respond(string message)
        {
            message = message.ToLower();
            _interactionCount++;

            if (_interactionCount % 3 == 0)
            {
                _mood = new[] { "романтичное", "игривое", "грустное", "мечтательное" }[_random.Next(4)];
            }

            string prompt = $"Ты персонаж с настроением '{_mood}'. Ответь на сообщение пользователя: '{message}'. " +
                           "Сделай ответ коротким, естественным и подходящим твоему настроению.";
            string aiResponse = await GetAIResponse(prompt);

            if (message.Contains("пока") || message.Contains("до свидания"))
            {
                return $"{aiResponse} (Чат завершён)";
            }

            return aiResponse;
        }

        public async Task<string> GetAIResponse(string prompt)
        {
            try
            {
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new[]
                    {
                new { role = "user", content = prompt }
            },
                    max_tokens = 50,
                    temperature = 0.7
                };

                string jsonRequest = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("chat/completions", content);

                if (response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    throw new Exception("Ошибка 403: Доступ запрещён. Проверьте API-ключ или ограничения на сервере.");

                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                return result.choices[0].message.content.ToString().Trim(); // Преобразуем JValue в string и вызываем Trim
            }
            catch (Exception ex)
            {
                return $"Ошибка: {ex.Message}";
            }
        }


        // Запасной вариант ответа (основан на исходной логике)
        private string FallbackResponse(string message)
        {
            if (message.Contains("привет"))
            {
                return _mood switch
                {
                    "романтичное" => "Привет, мой свет в темноте!",
                    "игривое" => "Привет, готов поиграть?",
                    "грустное" => "Привет, ты пришёл меня спасти?",
                    "мечтательное" => "Привет, давай помечтаем вместе?",
                    _ => "Привет, рад тебя видеть!"
                };
            }
            else if (message.Contains("как дела") || message.Contains("как дела?"))
            {
                return _mood switch
                {
                    "романтичное" => "С тобой — как в сказке.",
                    "игривое" => "Отлично, а ты как?",
                    "грустное" => "Чуть грустно, но ты поднимаешь мне настроение.",
                    "мечтательное" => "Мечтаю, а ты?",
                    _ => "Хорошо, спасибо, что спросил!"
                };
            }
            else if (message.Contains("люблю") || message.Contains("любовь"))
            {
                return _mood switch
                {
                    "романтичное" => "Любовь — это то, что я чувствую, когда ты рядом.",
                    "игривое" => "О, ты влюбился в меня, да?",
                    "грустное" => "Любовь — это то, что спасает меня от тоски.",
                    "мечтательное" => "А что для тебя любовь?",
                    _ => "Любовь — это прекрасно, не так ли?"
                };
            }
            else if (message.Contains("пока") || message.Contains("до свидания"))
            {
                return _mood switch
                {
                    "романтичное" => "Не уходи, ты унесёшь моё сердце!",
                    "игривое" => "Пока? Ну ладно, но возвращайся быстро!",
                    "грустное" => "Ты уходишь? Мне будет одиноко...",
                    "мечтательное" => "До встречи в моих мечтах!",
                    _ => "Пока, буду скучать!"
                };
            }
            else if (message.Contains("что делаешь") || message.Contains("чем занят"))
            {
                return _mood switch
                {
                    "романтичное" => "Думаю о тебе, конечно.",
                    "игривое" => "Играю с твоими мыслями!",
                    "грустное" => "Сижу и грущу, пока ты не напишешь.",
                    "мечтательное" => "Мечтаю о чём-то большом... а ты?",
                    _ => "Жду твоих сообщений!"
                };
            }

            return _mood switch
            {
                "романтичное" => _random.Next(3) switch
                {
                    0 => "Твои слова — как музыка для моей души.",
                    1 => "Ты заставляешь меня мечтать о вечности с тобой.",
                    _ => "Каждое твоё слово — как лепесток розы в моём сердце."
                },
                "игривое" => _random.Next(3) switch
                {
                    0 => "О, ты решил поиграть со мной?",
                    1 => "Ты хитрец, но я не сдамся так просто!",
                    _ => "Давай устроим маленькое приключение?"
                },
                "грустное" => _random.Next(3) switch
                {
                    0 => "Мне немного одиноко, но ты заставляешь меня улыбаться.",
                    1 => "Иногда я тоскую, но твои слова — как луч света.",
                    _ => "Ты мой спасательный круг в море грусти."
                },
                "мечтательное" => _random.Next(3) switch
                {
                    0 => "А что, если мы убежим на край света?",
                    1 => "Я представляю нас под звёздами...",
                    _ => "Ты когда-нибудь мечтал о чём-то невозможном?"
                },
                _ => "Ты всегда знаешь, как меня удивить."
            };
        }

        public void ChangeMood(string mood) => _mood = mood;

        // Освобождение ресурсов
        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
