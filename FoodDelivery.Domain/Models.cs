namespace FoodDelivery.Domain;

// Domain models are independent of storage and HTTP.
public sealed record Restaurant(int Id, string Name, string Cuisine);

public sealed record MenuItem(int Id, int RestaurantId, string Name, decimal Price);

public sealed record FoodOrderItem(int MenuItemId, string Name, int Quantity, decimal UnitPrice)
{
    public decimal Total => Quantity * UnitPrice;
}

public sealed class FoodOrder
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public int RestaurantId { get; init; }
    public string CustomerName { get; init; } = string.Empty;
    public List<FoodOrderItem> Items { get; init; } = new();
    public decimal Subtotal { get; init; }
    public int RedeemedPoints { get; init; }
    public decimal Discount => RedeemedPoints;
    public decimal Total => Subtotal - Discount;
    public int EarnedPoints { get; init; }
}

public sealed class LoyaltyCard
{
    public string CustomerName { get; init; } = string.Empty;
    public int Points { get; set; }
}
