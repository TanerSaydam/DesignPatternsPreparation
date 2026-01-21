using Polly;
using Polly.Retry;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/get-todos", async (HttpClient httpClient) =>
{
    string endpoint = "https://jsonplaceholder.typicode.com/todos";
    var res = await PipelineService.HttpPipeline.ExecuteAsync(async ct => await httpClient.GetAsync(endpoint, ct));

    res.EnsureSuccessStatusCode();

    var todos = await res.Content.ReadFromJsonAsync<List<Todo>>();

    return todos;
});

app.Run();

record Todo(
    int UserId,
    int Id,
    string Title,
    bool Completed);

public class PipelineService
{
    public static ResiliencePipeline<HttpResponseMessage> HttpPipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
    .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
    {
        MaxRetryAttempts = 3, //deneme süresi
        Delay = TimeSpan.FromSeconds(5), //her denemede bu kadar bekle
        ShouldHandle = new PredicateBuilder<HttpResponseMessage>() //sonucu nasıl işleyeceği
            .Handle<HttpRequestException>()
            .HandleResult(r => !r.IsSuccessStatusCode) // 404/500 vs retry
    })
    .AddTimeout(TimeSpan.FromSeconds(20)) //Timeout, bir deneme 20 saniyeyi aşarsa iptal eder.
    .Build();
}