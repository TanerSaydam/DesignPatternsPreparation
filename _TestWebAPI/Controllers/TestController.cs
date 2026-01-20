using Microsoft.AspNetCore.Mvc;

namespace _TestWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly INotification _notification;

    public TestController([FromKeyedServices(NotificationTypeEnum.Sms)] INotification notification)
    {
        _notification = notification;
    }

    [HttpGet("factory-pattern")]
    public IActionResult FactoryPattern()
    {
        _notification.Send("Hello world");
        return Ok(new { Message = "Is successful" });
    }
}
