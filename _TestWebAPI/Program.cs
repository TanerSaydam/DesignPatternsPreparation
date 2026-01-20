using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region Singleton DI
builder.Services.AddSingleton<DISingletonClass>();
#endregion

#region Factory DI
builder.Services.AddKeyedScoped<INotification, SmsNotification>(NotificationTypeEnum.Sms);
builder.Services.AddKeyedScoped<INotification, EmailNotification>(NotificationTypeEnum.Email);
#endregion

var app = builder.Build();

#region Singleton Endpoint
app.MapGet("singleton-pattern", ([FromServices] DISingletonClass dISingletonClass) =>
{
    var res = dISingletonClass.VerifyTCNumber("111");

    return res;
});
#endregion

#region Factory Endpoint

app.MapGet("factory-pattern", ([FromKeyedServices(NotificationTypeEnum.Sms)] INotification notification) =>
{
    notification.Send("Hello world");
    return Results.Ok(new { Message = "Is successful" });
});
#endregion

app.MapControllers();

app.Run();

#region Singleton DI
class DISingletonClass
{
    public bool VerifyTCNumber(string tcNo)
    {
        Console.WriteLine("{0} TC no is {1}", tcNo, true);
        return true;
    }
}
#endregion

#region Factory Initialize
public interface INotification
{
    void Send(string message);
}

public class SmsNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine("Sending sms... Message is {0}", message);
    }
}

public class EmailNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine("Sending email... Message is {0}", message);
    }
}

public enum NotificationTypeEnum
{
    Sms,
    Email,
}
#endregion


//Design Principle
//Yazılım tasarlarken uyman gereken temel kurallar / felsefeler / rehberler
//Daha esnek
//Daha bakımı kolay
//Daha genişletilebilir
//Daha test edilebilir
//SOLID, DRY, KISS, YAGNI, Separation of Concerns

//🧩 Principle vs Pattern farkı
//Şey	Ne?
//Principle	“Nasıl düşünmeliyim?”
//Pattern	“Bu problemi nasıl çözerim?”
//Framework	“Bunu hazır veriyorum, kullan”

//🏗️ Architectural Pattern nedir?

//Architectural Pattern = Uygulamanın genel iskeletini ve katmanlı yapısını tanımlayan büyük ölçekli tasarım şablonudur.

//Yani:

//❌ Bir class’ın içi değil

//❌ Bir metodun nasıl yazıldığı değil

//✅ Sistemin tamamı nasıl organize edilir? sorusunun cevabı