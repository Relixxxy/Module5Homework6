using System.Net;
using Basket.Host.Models.Requests;
using Basket.Host.Models.Response;
using Basket.Host.Services.Interfaces;
using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope("basket.basket")]
[Route(ComponentDefaults.DefaultRoute)]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(AddItemResponse<int?>), (int)HttpStatusCode.OK)]
    public async Task<AddItemResponse<int?>> AddProductToCatalog(CreateProductRequest request)
    {
        var result = await _basketService.AddCatalogItem(request);
        return new AddItemResponse<int?>() { Id = result };
    }
}
