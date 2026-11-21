namespace FoodDelivery.Domain;

// Interfaces belong to Domain; concrete implementations belong to Infrastructure.
public interface IRestaurantRepository
{
    IReadOnlyList<Restaurant> GetAll();
    Restaurant? GetById(int id);
    IReadOnlyList<MenuItem> GetMenu(int restaurantId);
    MenuItem? GetMenuItem(int id);
}

public interface IOrderRepository
{
    IReadOnlyList<FoodOrder> GetAll();
    FoodOrder? GetById(Guid id);
    void Add(FoodOrder order);
}

public interface ILoyaltyCardRepository
{
    LoyaltyCard GetOrCreate(string customerName);
}
