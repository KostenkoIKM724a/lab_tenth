using FoodDelivery.Application;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public sealed class RestaurantsController : ControllerBase
{
    private readonly CatalogService _catalog;

    public RestaurantsController(CatalogService catalog)
    {
        _catalog = catalog;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_catalog.GetRestaurants());

    [HttpGet("{id:int}/menu")]
    public IActionResult GetMenu(int id)
    {
        if (_catalog.GetRestaurant(id) is null)
            return NotFound(new { error = "Ресторан не знайдено." });
        return Ok(_catalog.GetMenu(id));
    }
}
