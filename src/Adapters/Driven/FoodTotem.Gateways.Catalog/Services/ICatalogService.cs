using FoodTotem.Gateways.Catalog.ViewModels;

namespace FoodTotem.Gateways.Catalog.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogViewModel>> GetCatalogByCategory(string category);
        Task<CatalogViewModel> CreateCatalogItem(CreateCatalogViewModel catalogItem);
        Task<CatalogViewModel> UpdateCatalogItem(Guid catalogId, CreateCatalogViewModel catalogItem);
        Task DeleteCatalogItem(Guid id);
    }
}