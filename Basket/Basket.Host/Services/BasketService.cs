using Basket.Host.Models.Requests;
using Basket.Host.Models.Response;
using Basket.Host.Services.Interfaces;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Basket.Host.Services;

public class BasketService : IBasketService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly ILogger<BasketService> _logger;
    private readonly IInternalHttpClientService _httpClient;

    public BasketService(ILogger<BasketService> logger, IInternalHttpClientService httpClient, IOptions<AppSettings> settings)
    {
        _logger = logger;
        _httpClient = httpClient;
        _settings = settings;
    }

    public async Task<int?> AddCatalogItem(CreateProductRequest item)
    {
        var result = await _httpClient.SendAsync<AddItemResponse<int?>, CreateProductRequest>(
            $"{_settings.Value.CatalogUrl}/add",
            HttpMethod.Post,
            item);

        _logger.LogInformation(result.ToString());

        return result.Id;
    }
}
