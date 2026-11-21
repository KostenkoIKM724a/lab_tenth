using FoodDelivery.Domain;

namespace FoodDelivery.Application;

public sealed class CatalogService
{
    private readonly IRestaurantRepository _restaurants;

    public CatalogService(IRestaurantRepository restaurants)
    {
        _restaurants = restaurants;
    }

    public IReadOnlyList<Restaurant> GetRestaurants() => _restaurants.GetAll();
    public Restaurant? GetRestaurant(int id) => _restaurants.GetById(id);
    public IReadOnlyList<MenuItem> GetMenu(int id) => _restaurants.GetMenu(id);
}
