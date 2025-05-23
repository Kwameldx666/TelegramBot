﻿using Telegram.Bot;
using TelegramBot.Progress.Models;

namespace TelegramBot.Progress.Strategy
{
    public class FlirtSparkStrategy : IStrategy
    {
        private readonly string[] _flirtLines = {
    "Ты украла мое сердце, и я даже не буду его искать!",
    "Если бы ты была звездой, я бы смотрел в небо всю ночь!",
    "Ты — мой главный баг, который я не хочу исправлять!",
    "Твой взгляд — мой любимый баг в системе реальности!",
    "Если бы красота была преступлением, тебя бы уже арестовали!",
    "Ты заставляешь мое сердце работать в режиме overclocking!",
    "Ты — идеальный алгоритм моего счастья!",
    "Ты — мой самый важный update в жизни!",
    "Ты пишешь код моей души!",
    "Ты — единственный exception, который я люблю!",
    "Ты освещаешь мою жизнь, как 1000 люмен!",
    "Ты заставляешь мои сердечные запросы выполняться мгновенно!",
    "Ты — мой самый ценный актив!",
    "Ты заставляешь мои переменные изменяться!",
    "Ты — моя идеальная библиотека!",
    "Без тебя моя жизнь — null!",
    "Ты — мой самый стабильный commit!",
    "Ты — мой идеальный патч для серого дня!",
    "Ты — исключение, которое делает мой код идеальным!",
    "Ты — мой главный dependency в жизни!",
    "Ты управляешь моими запросами на 100%!",
    "Ты — моя самая важная SQL-запрос!",
    "Ты — лучшая версия всех апдейтов!",
    "Ты заставляешь мой процессор перегреваться!",
    "Ты — моя главная функция счастья!",
    "Ты — мой бесконечный цикл радости!",
    "Ты заставляешь мои пакеты зависеть только от тебя!",
    "Ты — идеальный класс, от которого я хочу наследоваться!",
    "Ты — мой исключительный try-catch!",
    "Ты — моя главная строка кода!",
    "Ты — мой единственный return в жизни!",
    "Ты и есть главная причина моего дебага!",
    "Ты заставляешь мой код работать без багов!",
    "Ты — идеальный алгоритм для моего счастья!",
    "Ты добавила в мою жизнь правильный параметр!",
    "Ты компилируешь мою жизнь в счастливый результат!",
    "Ты — мой любимый shortcut к счастью!",
    "Ты заставляешь мой сервер работать без сбоев!",
    "Ты заставляешь мое сердце работать без лагов!",
    "Ты моя самая красивая переменная!",
    "Ты — мой лучший import в жизни!",
    "Ты оптимизируешь мою жизнь!",
    "Ты — мой правильный алгоритм!",
    "Ты делаешь мою логику безупречной!",
    "Ты вызываешь у меня стек переполнения эмоций!",
    "Ты превращаешь мои ошибки в успех!",
    "Ты единственный ключ к моему счастью!",
    "Ты делаешь мой код элегантным!",
    "Ты мой главный инстанс любви!",
    "Ты заставляешь меня генерировать только положительные эмоции!",
    "Ты — лучший AI, который я когда-либо встречал!",
    "Ты всегда в топе моего стека!",
    "Ты как CSS — добавляешь стиль в мою жизнь!",
    "Ты — моя конечная точка API счастья!",
    "Ты заставляешь меня компилироваться без ошибок!",
    "Ты лучший merge, который случился в моей жизни!",
    "Ты — идеальный push в моем репозитории!",
    "Ты заполняешь мои пропущенные теги счастья!",
    "Ты — моя любимая зависимость!",
    "Ты добавляешь в мою жизнь цветовую схему радости!",
    "Ты — мой любимый UI-компонент!",
    "Ты — мой хостинг в этом мире!",
    "Ты — мое лучшее расширение возможностей!",
    "Ты освещаешь мой код, как подсветка синтаксиса!",
    "Ты исправляешь мои баги одним взглядом!",
    "Ты — мой самый красивый регулярный выражение!",
    "Ты делаешь мой день лучше, как auto-save!",
    "Ты — мой любимый breakpoint в жизни!",
    "Ты заставляешь мое сердце работать без зависаний!",
    "Ты — идеальный refactoring для моего сердца!",
    "Ты — мой самый важный протокол безопасности!",
    "Ты — моя самая успешная сборка!",
    "Ты мой самый удачный deploy!",
    "Ты ускоряешь мой рендеринг счастья!",
    "Ты мое идеальное сочетание HTML и CSS!",
    "Ты дополняешь мой код, как идеальный плагин!",
    "Ты синхронизируешь мое сердце с радостью!",
    "Ты добавляешь в мою жизнь новые API!",
    "Ты моя главная функция в этом мире!",
    "Ты компилируешь мои мечты в реальность!",
    "Ты обеспечиваешь мою бесперебойную работу!",
    "Ты — мой лучший rollback из всех ошибок!",
    "Ты заставляешь мой кеш сохранять только лучшие моменты!",
    "Ты даешь мне стабильную связь с счастьем!",
    "Ты мой лучший сервер в облаке любви!",
    "Ты поддерживаешь мою жизнь, как надежный бекенд!",
    "Ты мой стабильный биллинг счастья!",
    "Ты создаешь в моей жизни идеальный UX!",
    "Ты обновляешь мою систему до лучшей версии!",
    "Ты делаешь мою жизнь такой же красивой, как анимация в CSS!",
    "Ты — мой самый ценный токен!",
    "Ты — лучший API-запрос, который я когда-либо отправлял!",
    "Ты мой самый удачный hotfix!",
    "Ты — мой самый надежный алгоритм шифрования счастья!",
    "Ты моя любимая строка в коде жизни!"
};

        private readonly Random _rand = new Random();

        public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId, UserState state, string input, CancellationToken cancellationToken)
        {
            string flirt = _flirtLines[_rand.Next(_flirtLines.Length)];
            await botClient.SendTextMessageAsync(chatId, flirt, cancellationToken: cancellationToken);
        }


    }
}
