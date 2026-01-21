using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

var builder = WebApplication.CreateBuilder(args); //Uygulamanın konfigürasyonunu toplar

#region Service Registration / Service Configuration Phase / DI Configuration / Composition Root
//builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

//builder.Configuration.AddJsonFile(
//    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
//    optional: true,
//    reloadOnChange: true);

//string? value = builder.Configuration.GetSection("AppLevel").Value;
//Console.WriteLine(value ?? "None");
#endregion

var app = builder.Build(); //uygulamanın çalışabilir bir instance'ini oluşturur // DI Container’ı BARINDIRAN ve onu KULLANAN uygulama nesnesidir

await new VaultService().GetSecrets();
var connectionString = VaultService.Datas[VaultDataNames.SqlConnectionString];
Console.WriteLine("Connection String: {0}", connectionString);

#region Middleware pipeline configuration

//ASP.NET Core middleware pipeline, Chain of Responsibility fikrinin web request yaşam döngüsüne uyarlanmış halidir
app.Use((context, next) =>
{
    return next(); //Task dönüyor
    //return next(context);//eski version
});

app.Use(async (context, next) =>
{
    await next(); //taskı compile üretiyor
});

//async YOKSA → Task’ı sen return edersin
//async VARSA → Task’ı compiler üretir, sen sadece await edersin

app.MapGet("test", () => { });

#endregion

app.Run();

//async Task Foo1()
//{
//    await DoSomething();
//}

//Task Foo2()
//{
//    var t = new Task(...);
//    return t;
//}

public class VaultService
{
    public static Dictionary<string, object> Datas = new();
    public async Task GetSecrets()
    {
        var vaultToken = "root";
        var vaultUri = "http://127.0.0.1:8200";
        var vaultTokenInfo = new TokenAuthMethodInfo(vaultToken);
        var vaultClientSettings = new VaultClientSettings(vaultUri, vaultTokenInfo);
        var vaultClient = new VaultClient(vaultClientSettings);

        var secrets = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(
            path: "TestWebAPI",
            mountPoint: "secret");

        Datas = secrets.Data.Data.ToDictionary();
    }
}

public class VaultDataNames
{
    public static string SqlConnectionString = "ConnectionStrings::SqlServer";
}