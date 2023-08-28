using FoodTotem.Gateways.Http;
using FoodTotem.Gateways.MercadoPago.ViewModels;

namespace FoodTotem.Gateways.MercadoPago
{
    public class MercadoPagoPaymentService : IMercadoPagoPaymentService
    {
        private readonly IHttpHandler _httpHandler;

        private const string ACCESS_TOKEN = "Bearer TEST-1955353689572708-082708-31814bc61dd1d32aad491ac0c0a23a98-1462421678";
        private const string EXTERNAL_POS_ID = "FTSPOS01";
        private const string USER_ID = "1462421678";
        private const string BASE_URL = "https://api.mercadopago.com";

        public MercadoPagoPaymentService(IHttpHandler httpHandler)
        {
            _httpHandler = httpHandler;
        }

        public Task<QRCodeViewModel> GetPaymentQRCode(PaymentInformationViewModel paymentInformationViewModel)
        {
            var uri = $"{BASE_URL}/instore/orders/qr/seller/collectors/{USER_ID}/pos/{EXTERNAL_POS_ID}/qrs";
            return _httpHandler.PutAsync<PaymentInformationViewModel,QRCodeViewModel>(uri, paymentInformationViewModel, MountMercadoPagoHeader());
        }

        private Dictionary<string, string> MountMercadoPagoHeader()
        {
            return new Dictionary<string, string>()
            {
                { "Authorization", ACCESS_TOKEN }
            };
        }
    }
}
