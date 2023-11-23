using FoodTotem.Gateways.Catalog.ViewModels;
using FoodTotem.Gateways.Http;
using Microsoft.Extensions.Configuration;

namespace FoodTotem.Gateways.Catalog.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IHttpHandler _httpHandler;
        private readonly IConfiguration _configuration;

        private readonly string CatalogUrl;

        public CatalogService(IHttpHandler httpHandler, IConfiguration configuration)
        {
            _httpHandler = httpHandler;
            _configuration = configuration;
            CatalogUrl = _configuration.GetSection("CatalogServiceUrl").Value;
        }

        public async Task<IEnumerable<CatalogViewModel>> GetCatalogByCategory(string category)
        {
            return await _httpHandler.GetAsync<IEnumerable<CatalogViewModel>>($"{CatalogUrl}/food?category={category}", null);
        }

        public async Task<CatalogViewModel> CreateCatalogItem(CreateCatalogViewModel catalogItem)
        {
            return await _httpHandler.PostAsync<CreateCatalogViewModel,CatalogViewModel>($"{CatalogUrl}/food", catalogItem, null);
        }

        public async Task<CatalogViewModel> UpdateCatalogItem(Guid catalogId, CreateCatalogViewModel catalogItem)
        {
            return await _httpHandler.PutAsync<CreateCatalogViewModel,CatalogViewModel>($"{CatalogUrl}/food/{catalogId}", catalogItem, null);
        }

        public async Task DeleteCatalogItem(Guid id)
        {
            await _httpHandler.DeleteAsync($"{CatalogUrl}/food/{id}", null);
        }
    }
}