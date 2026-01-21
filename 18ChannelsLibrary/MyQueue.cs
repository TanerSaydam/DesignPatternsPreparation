using System.Threading.Channels;

namespace _18ChannelsLibrary;

public class MyQueue
{
    public readonly Channel<EmailDto> _channel;
    public MyQueue()
    {
        _channel = Channel.CreateBounded<EmailDto>(
            new BoundedChannelOptions(1)
            {
                SingleReader = true, //tek consumer,
                SingleWriter = true, //Bu kanala sadece tek bir thread yazacak. Diğer producer’lardan hiçbiri asla yazmayacak Bu bilgi sayesinde Channel kilitsiz (lock-free) çok daha hızlı bir yazma yolu seçiyor.
            }
        );
    }
}

public sealed record EmailDto(
    string From,
    string Subject);
