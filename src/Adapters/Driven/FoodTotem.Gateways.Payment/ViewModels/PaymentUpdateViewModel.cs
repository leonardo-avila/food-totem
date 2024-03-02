using System.Diagnostics.CodeAnalysis;

namespace FoodTotem.Gateways.Payment.ViewModels;
    
[ExcludeFromCodeCoverage]
public class PaymentUpdateViewModel
{
    public string Id { get; set; }
    public bool IsApproved { get; set; }
}