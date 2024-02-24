using CoreLibrary.Models.Setting;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;

namespace CoreLibrary.Utilities.TelegramBot;

public class TelegramService : ITelegramService
{
    private readonly TelegramBotClient? _telegramBotClient;
    private readonly string targetChatId = string.Empty;

    public TelegramService(IOptions<ConfigurationValues> configuration)
    {
       _telegramBotClient = new TelegramBotClient(configuration.Value.Telegram.Token);
       targetChatId = configuration.Value.Telegram.ChatId;
    }
    
    public async Task SendMessageAsync(long chatId, string msg)
    {
        try
        {
            await _telegramBotClient!.SendTextMessageAsync(
                chatId: chatId,
                text: msg
            );
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
       
    }

    public async Task SendMessageAsync(string msg)
    {
        try
        {
            await _telegramBotClient!.SendTextMessageAsync(
                chatId: targetChatId,
                text: msg
            );
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task SendImageAsync(long chatId, string imagePath, string caption)
    {
        try
        {
            await using FileStream photoStream = new FileStream( imagePath, FileMode.Open);
        
            await _telegramBotClient!.SendPhotoAsync(
                chatId: chatId,
                photo: new InputOnlineFile(photoStream),
                caption: caption
            );
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public async Task SendImageAsync(string imagePath, string caption)
    {
        try
        {
            await using FileStream photoStream = new FileStream( imagePath, FileMode.Open);
        
            await _telegramBotClient!.SendPhotoAsync(
                chatId: targetChatId,
                photo: new InputOnlineFile(photoStream),
                caption: caption
            );
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
        }
    }
}