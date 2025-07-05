# Telegram Bot Token Configuration

## Местоположение токена / Token Location

Токен Telegram бота теперь находится в файле конфигурации `appsettings.json`, а НЕ в исходном коде.

The Telegram bot token is now located in the configuration file `appsettings.json`, NOT in the source code.

## Настройка / Setup

1. **Скопируйте файл примера / Copy the example file:**
   ```bash
   cp appsettings.example.json appsettings.json
   ```

2. **Отредактируйте appsettings.json / Edit appsettings.json:**
   ```json
   {
     "TelegramBot": {
       "Token": "YOUR_BOT_TOKEN_HERE"
     }
   }
   ```

3. **Замените `YOUR_BOT_TOKEN_HERE` на ваш реальный токен бота / Replace `YOUR_BOT_TOKEN_HERE` with your actual bot token**

## Безопасность / Security

⚠️ **ВАЖНО / IMPORTANT:**

- Файл `appsettings.json` добавлен в `.gitignore` и НЕ должен попадать в репозиторий
- The `appsettings.json` file is added to `.gitignore` and should NOT be committed to the repository
- Никогда не публикуйте ваш токен бота в открытом доступе
- Never publish your bot token publicly

## Изменения в коде / Code Changes

Токен теперь читается из конфигурации в классе `BotClientSingleton`:
- Файл: `Progress/Singleton/BotClientSingleton.cs`
- Метод: конструктор использует Microsoft.Extensions.Configuration

The token is now read from configuration in the `BotClientSingleton` class:
- File: `Progress/Singleton/BotClientSingleton.cs`  
- Method: constructor uses Microsoft.Extensions.Configuration

## Запуск / Running

Убедитесь, что файл `appsettings.json` существует и содержит корректный токен перед запуском бота.

Make sure the `appsettings.json` file exists and contains a valid token before running the bot.