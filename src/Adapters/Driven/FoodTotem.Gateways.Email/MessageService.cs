using System.Net;
using System.Net.Mail;
using FoodTotem.Identity.Domain.Ports;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FoodTotem.Gateways.Email;

public class MessageService : IMessageService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<MessageService> _logger;

    public MessageService(IConfiguration configuration, ILogger<MessageService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void SendMessageAsync(string email, string subject, string message)
    {
        try {
        var client = new SmtpClient(_configuration["Mailtrap:Host"], 2525)
        {
            Credentials = new NetworkCredential(_configuration["Mailtrap.Username"], _configuration["Mailtrap.Password"]),
            EnableSsl = false
        };
        client.Send("foodtotem@teste.com", email, subject, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
        }
    }
}