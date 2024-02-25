namespace FoodTotem.Identity.Domain.Ports;

public interface IMessageService {
    void SendMessageAsync(string email, string subject, string message);
}