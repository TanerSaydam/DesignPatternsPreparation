using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Factory Pattern");

string msg = "Hello world";

#region Old Version
//SmsNotification smsNotification = new();
//smsNotification.Send(msg);

//EmailNotification emailNotification = new();
//emailNotification.Send(msg);

//INotification smsNotification = new SmsNotification();
//smsNotification.Send(msg);

//INotification notification = NotificationFactory.Create(NotificationTypeEnum.Sms);
//notification.Send(msg);
#endregion

#region DI Version
ServiceCollection services = new();
services.AddKeyedScoped<INotification, SmsNotification>(NotificationTypeEnum.Sms);
services.AddKeyedScoped<INotification, EmailNotification>(NotificationTypeEnum.Email);

var srv = services.BuildServiceProvider();
var notification = srv.GetRequiredKeyedService<INotification>(NotificationTypeEnum.Email);
notification.Send(msg);
#endregion

#region Initialize
interface INotification
{
    void Send(string message);
}

class SmsNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine("Sending sms... Message is {0}", message);
    }
}

class EmailNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine("Sending email... Message is {0}", message);
    }
}

enum NotificationTypeEnum
{
    Sms,
    Email,
}

#endregion

#region Old Version
class NotificationFactory
{
    public static INotification Create(NotificationTypeEnum type)
    {
        //if (type == "sms") return new SmsNotification();
        //else if (type == "email") return new EmailNotification();

        switch (type)
        {
            case NotificationTypeEnum.Sms: return new SmsNotification();
            case NotificationTypeEnum.Email: return new EmailNotification();
            default: throw new ArgumentNullException(type.ToString());
        }
    }
}

#endregion