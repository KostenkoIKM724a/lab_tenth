using FoodDelivery.Domain;

namespace FoodDelivery.Infrastructure;

// Lab 10 uses in-memory storage. EF Core/SQL Server will be added in Lab 12.
public sealed class InMemoryRestaurantRepository : IRestaurantRepository
{
    private readonly List<Restaurant> _restaurants = new()
    {
        new(1, "Смак Дому", "Українська кухня"),
        new(2, "Тісто і Вогонь", "Піца"),
        new(3, "Рис і Вок", "Азійська кухня")
    };

    private readonly List<MenuItem> _menu = new()
    {
        new(101, 1, "Борщ", 135m),
        new(102, 1, "Вареники", 170m),
        new(103, 1, "Компот", 45m),
        new(201, 2, "Маргарита", 240m),
        new(202, 2, "Пепероні", 285m),
        new(203, 2, "Лимонад", 65m),
        new(301, 3, "Локшина з куркою", 220m),
        new(302, 3, "Рис з овочами", 180m),
        new(303, 3, "Зелений чай", 55m)
    };

    public IReadOnlyList<Restaurant> GetAll() => _restaurants.ToList();
    public Restaurant? GetById(int id) => _restaurants.FirstOrDefault(x => x.Id == id);
    public IReadOnlyList<MenuItem> GetMenu(int restaurantId) =>
        _menu.Where(x => x.RestaurantId == restaurantId).ToList();
    public MenuItem? GetMenuItem(int id) => _menu.FirstOrDefault(x => x.Id == id);
}

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly List<FoodOrder> _orders = new();
    private readonly object _sync = new();

    public IReadOnlyList<FoodOrder> GetAll()
    {
        lock (_sync)
        {
            return _orders.ToList();
        }
    }

    public FoodOrder? GetById(Guid id)
    {
        lock (_sync)
        {
            return _orders.FirstOrDefault(x => x.Id == id);
        }
    }

    public void Add(FoodOrder order)
    {
        lock (_sync)
        {
            _orders.Add(order);
        }
    }
}

public sealed class InMemoryLoyaltyCardRepository : ILoyaltyCardRepository
{
    private readonly Dictionary<string, LoyaltyCard> _cards =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["Олена"] = new LoyaltyCard { CustomerName = "Олена", Points = 80 }
        };

    private readonly object _sync = new();

    public LoyaltyCard GetOrCreate(string customerName)
    {
        lock (_sync)
        {
            if (!_cards.TryGetValue(customerName, out var card))
            {
                card = new LoyaltyCard { CustomerName = customerName, Points = 0 };
                _cards[customerName] = card;
            }
            return card;
        }
    }
}
