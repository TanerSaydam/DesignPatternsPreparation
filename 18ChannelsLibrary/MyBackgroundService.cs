namespace _18ChannelsLibrary;

public sealed class MyBackgroundService(MyQueue myQueue) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var item in myQueue._channel.Reader.ReadAllAsync())
        {
            Console.WriteLine("Email sended. Email Information:\nFrom: {0}\nSubject: {1}", item.From, item.Subject);
            await Task.Delay(500);
        }
    }
}
