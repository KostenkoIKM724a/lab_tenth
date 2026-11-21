namespace FoodDelivery.Application;

public sealed class CreateOrderRequest
{
    public string CustomerName { get; set; } = string.Empty;
    public int RestaurantId { get; set; }
    public List<CreateOrderLine> Items { get; set; } = new();
    public int RedeemPoints { get; set; }
}

public sealed class CreateOrderLine
{
    public int MenuItemId { get; set; }
    public int Quantity { get; set; }
}
