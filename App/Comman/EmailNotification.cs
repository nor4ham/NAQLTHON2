using Newtonsoft.Json;

public class EmailNotificationMessage
{
    public string Recipient { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}

public class EmailNotificationService
{
    private readonly RabbitMQService _rabbitMQService;

    public EmailNotificationService(RabbitMQService rabbitMQService)
    {
        _rabbitMQService = rabbitMQService;
    }

    public void SendEmailNotification(string recipient, string subject, string body)
    {
        var emailMessage = new EmailNotificationMessage
        {
            Recipient = recipient,
            Subject = subject,
            Body = body
        };

        // Serialize the message as JSON and send it to the RabbitMQ queue
        var message = JsonConvert.SerializeObject(emailMessage);
        _rabbitMQService.SendMessage("email_queue", message);
    }
}
