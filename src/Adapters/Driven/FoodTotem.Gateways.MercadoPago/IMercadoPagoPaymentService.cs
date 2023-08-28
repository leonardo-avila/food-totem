using FoodTotem.Gateways.MercadoPago.ViewModels;

namespace FoodTotem.Gateways.MercadoPago
{
	public interface IMercadoPagoPaymentService
	{
		Task<QRCodeViewModel> GetPaymentQRCode(PaymentInformationViewModel paymentInformationViewModel);
	}
}

