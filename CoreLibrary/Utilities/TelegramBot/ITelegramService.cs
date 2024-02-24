namespace CoreLibrary.Utilities.TelegramBot;

public interface ITelegramService
{
    Task SendMessageAsync(long chatId, string msg);
    Task SendMessageAsync(string msg);
    Task SendImageAsync(long chatId, string imagePath, string caption);
    Task SendImageAsync(string imagePath, string caption);
}