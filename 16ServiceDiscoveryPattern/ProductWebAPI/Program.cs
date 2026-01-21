using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddConsulDiscoveryClient();
builder.Services.AddHttpClient();

var app = builder.Build();

app.MapGet("/getall", async (IDiscoveryClient client, HttpClient httpClient, CancellationToken cancellationToken) =>
{
    var products = Product.List;

    var services = await client.GetInstancesAsync("CategoryWebAPI", cancellationToken);
    Uri categoryUri = services.FirstOrDefault()?.Uri ?? throw new ArgumentNullException(nameof(services));
    var endpoint = categoryUri + "getall";
    var categories = await httpClient.GetFromJsonAsync<List<CategoryDto>>(endpoint, cancellationToken);

    products.ForEach(val => val.CategoryName = categories?.FirstOrDefault(i => i.Id == val.CategoryId)?.Name ?? null);

    return products;
});

app.Run();

class Product
{
    public static List<Product> List = new()
    {
        new()
        {
            Id = 1,
            Name = "Masaüstü",
            CategoryId = 1,
        },
        new()
        {
            Id = 2,
            Name = "Laptop",
            CategoryId = 1,
        },
        new()
        {
            Id = 1,
            Name = "Iphone 17+",
            CategoryId = 3,
        }
    };
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
}

record CategoryDto(int Id, string Name);