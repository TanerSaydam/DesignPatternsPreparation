using _18ChannelsLibrary;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<MyQueue>();
builder.Services.AddHostedService<MyBackgroundService>();

var app = builder.Build();

app.MapGet("/", async (MyQueue myQueue) =>
{
    await myQueue._channel.Writer.WriteAsync(new EmailDto("tanersaydam@gmail.com", "Hello world"));
    return Results.Ok("Is completed");
});

app.Run();
