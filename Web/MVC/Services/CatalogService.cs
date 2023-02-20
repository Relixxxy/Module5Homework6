using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVC.Dtos;
using MVC.Models.Enums;
using MVC.Services.Interfaces;
using MVC.ViewModels;

namespace MVC.Services;

public class CatalogService : ICatalogService
{
    private readonly IOptions<AppSettings> _settings;
    private readonly IHttpClientService _httpClient;
    private readonly ILogger<CatalogService> _logger;

    public CatalogService(IHttpClientService httpClient, ILogger<CatalogService> logger, IOptions<AppSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
        _logger = logger;
    }

    public async Task<Catalog> GetCatalogItems(int page, int take, int? brand, int? type)
    {
        await TestBasketEndpoints();

        var filters = new Dictionary<CatalogTypeFilter, int>();

        if (brand.HasValue)
        {
            filters.Add(CatalogTypeFilter.Brand, brand.Value);
        }
        
        if (type.HasValue)
        {
            filters.Add(CatalogTypeFilter.Type, type.Value);
        }
        
        var result = await _httpClient.SendAsync<Catalog, PaginatedItemsRequest<CatalogTypeFilter>>($"{_settings.Value.CatalogUrl}/items",
           HttpMethod.Post, 
           new PaginatedItemsRequest<CatalogTypeFilter>()
            {
                PageIndex = page,
                PageSize = take,
                Filters = filters
            });

        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        await Task.Delay(300);
        var list = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "0",
                Text = "brand 1"
            },
            new SelectListItem()
            {
                Value = "1",
                Text = "brand 2"
            }
        };
        var result = await _httpClient.SendAsync<object, object>($"{_settings.Value.CatalogUrl}/getbrands",
            HttpMethod.Post, new {} );
        
        return list;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        await Task.Delay(300);
        var list = new List<SelectListItem>
        {
            new SelectListItem()
            {
                Value = "0",
                Text = "type 1"
            },
            
            new SelectListItem()
            {
                Value = "1",
                Text = "type 2"
            }
        };

        return list;
    }

    private async Task TestBasketEndpoints()
    {
        await _httpClient.SendAsync<object, object>($"{_settings.Value.BasketUrl}/testlog",
           HttpMethod.Post, null);

        await _httpClient.SendAsync<object, object>($"{_settings.Value.BasketUrl}/loguserid",
           HttpMethod.Post, null);
    }
}
