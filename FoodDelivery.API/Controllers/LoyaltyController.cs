using FoodDelivery.Application;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

[ApiController]
[Route("api/loyalty")]
public sealed class LoyaltyController : ControllerBase
{
    private readonly OrderService _orders;

    public LoyaltyController(OrderService orders)
    {
        _orders = orders;
    }

    [HttpGet("{customerName}")]
    public IActionResult GetPoints(string customerName)
    {
        try
        {
            var points = _orders.GetLoyaltyPoints(customerName);
            return Ok(new { customerName, points });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
