using System.Threading.Channels;

namespace Oid85.FinMarket.WebHost;

public class ChannelMessageQueue
{
    private readonly Channel<ChannelMessage> _channel = Channel.CreateBounded<ChannelMessage>(
        new BoundedChannelOptions(50)
        {
            FullMode = BoundedChannelFullMode.DropOldest
        });
    
    public async Task ReadAsync()
    {
        var timespan = TimeSpan.FromMinutes(1);
        
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(timespan);
            
        while (!cts.Token.IsCancellationRequested)
        {
            while (_channel.Reader.TryRead(out var message))
            {
                Console.WriteLine(message.Message);
            }
        }
        
        Console.WriteLine("Выход по таймауту");
    }
    
    public async Task WriteAsync()
    {
        for (int i = 0; i < 100; i++)
        {
            await _channel.Writer.WriteAsync(new ChannelMessage() { Message = $"{i}"});   
        }
    }
}