using _18ChannelsLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MyQueue>();
builder.Services.AddHostedService<MyBackgroundService>();

WeatherUI weatherUI = new();
WeaterSystem weaterSystem
    = new();
weaterSystem.Subscribe(weatherUI);

weaterSystem.SetWeater(25);

var app = builder.Build();

app.MapGet("/", async (MyQueue myQueue) =>
{
    await myQueue._channel.Writer.WriteAsync(new EmailDto("tanersaydam@gmail.com", "Hello world"));
    return Results.Ok("Is completed");
});



app.Run();

class WeaterSystem : IObservable<int>
{
    private List<IObserver<int>> _observers = new();
    private int _weater;
    public IDisposable Subscribe(IObserver<int> observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }

        return new Unsubsciber(_observers, observer);
    }

    public void SetWeater(int newWeater)
    {
        _weater = newWeater;
        Notify();
    }

    private void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(_weater);
        }
    }

    private class Unsubsciber : IDisposable
    {
        private List<IObserver<int>> _observers;
        private IObserver<int> _observer;

        public Unsubsciber(List<IObserver<int>> observers, IObserver<int> observer)
        {
            _observers = observers;
            _observer = observer;
        }
        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
            {
                _observers.Remove(_observer);
            }
        }
    }
}

class WeatherUI : IObserver<int>
{
    public void OnCompleted()
    {
        Console.WriteLine("Weater sistemi kapandý");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine("Hata:{0}", error.Message);
    }

    public void OnNext(int value)
    {
        Console.WriteLine("Weater güncellendi: {0}", value);
    }
}