using FoodDelivery.Application;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.API.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController : ControllerBase
{
    private readonly OrderService _orders;

    public OrdersController(OrderService orders)
    {
        _orders = orders;
    }

    [HttpGet]
    public IActionResult GetAll() => Ok(_orders.GetOrders());

    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        var order = _orders.GetOrder(id);
        return order is null ? NotFound() : Ok(order);
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateOrderRequest request)
    {
        try
        {
            var order = _orders.PlaceOrder(request);
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
