using Steeltoe.Discovery.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConsulDiscoveryClient();

var app = builder.Build();

app.MapGet("/getall", () =>
{
    return new List<Category>()
    {
        new()
        {
            Id = 1,
            Name = "Bilgisayar"
        },
        new()
        {
            Id = 2,
            Name = "Konsol"
        },
        new()
        {
            Id = 3,
            Name = "Telefon"
        }
    };
});

app.Run();

class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}