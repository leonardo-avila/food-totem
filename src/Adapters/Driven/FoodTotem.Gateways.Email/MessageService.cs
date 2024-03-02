using System.Net;
using System.Net.Mail;
using FoodTotem.Identity.Domain.Ports;
using Microsoft.Extensions.Configuration;

namespace FoodTotem.Gateways.Email;

public class MessageService : IMessageService
{
    private readonly IConfiguration _configuration;

    public MessageService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendMessageAsync(string email, string subject, string message)
    {
        var client = new SmtpClient(_configuration["Mailtrap:Host"], 2525)
        {
            Credentials = new NetworkCredential(_configuration["Mailtrap.Username"], _configuration["Mailtrap.Password"]),
            EnableSsl = false
        };
        client.Send("foodtotem@teste.com", email, subject, message);
    }
}