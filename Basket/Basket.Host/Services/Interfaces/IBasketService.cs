using Basket.Host.Models.Requests;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task<int?> AddCatalogItem(CreateProductRequest item);
}
