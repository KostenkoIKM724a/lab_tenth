using FoodDelivery.Domain;

namespace FoodDelivery.Application;

public sealed class OrderService
{
    private readonly IRestaurantRepository _restaurants;
    private readonly IOrderRepository _orders;
    private readonly ILoyaltyCardRepository _loyalty;
    private readonly object _sync = new();

    public OrderService(
        IRestaurantRepository restaurants,
        IOrderRepository orders,
        ILoyaltyCardRepository loyalty)
    {
        _restaurants = restaurants;
        _orders = orders;
        _loyalty = loyalty;
    }

    public IReadOnlyList<FoodOrder> GetOrders() => _orders.GetAll();
    public FoodOrder? GetOrder(Guid id) => _orders.GetById(id);

    public int GetLoyaltyPoints(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Вкажіть ім'я клієнта.");

        lock (_sync)
        {
            return _loyalty.GetOrCreate(customerName.Trim()).Points;
        }
    }

    public FoodOrder PlaceOrder(CreateOrderRequest request)
    {
        if (request is null)
            throw new ArgumentException("Дані замовлення відсутні.");
        if (string.IsNullOrWhiteSpace(request.CustomerName))
            throw new ArgumentException("Вкажіть ім'я клієнта.");
        if (_restaurants.GetById(request.RestaurantId) is null)
            throw new ArgumentException("Ресторан не знайдено.");
        if (request.Items is null || request.Items.Count == 0)
            throw new ArgumentException("Замовлення не може бути порожнім.");
        if (request.RedeemPoints < 0)
            throw new ArgumentException("Кількість списаних балів не може бути від'ємною.");

        var lines = new List<FoodOrderItem>();
        foreach (var item in request.Items)
        {
            if (item is null || item.Quantity < 1 || item.Quantity > 20)
                throw new ArgumentException("Кількість кожної страви має бути від 1 до 20.");
            var menuItem = _restaurants.GetMenuItem(item.MenuItemId);
            if (menuItem is null || menuItem.RestaurantId != request.RestaurantId)
                throw new ArgumentException("Страва відсутня в меню вибраного ресторану.");
            lines.Add(new FoodOrderItem(menuItem.Id, menuItem.Name, item.Quantity, menuItem.Price));
        }

        var subtotal = lines.Sum(item => item.Total);
        // Educational loyalty rule: 1 point = 1 UAH, redeem at most 20% of subtotal.
        var maxRedeemable = (int)decimal.Floor(subtotal * 0.20m);
        if (request.RedeemPoints > maxRedeemable)
            throw new ArgumentException("Балами можна оплатити не більше 20% вартості.");

        lock (_sync)
        {
            var card = _loyalty.GetOrCreate(request.CustomerName.Trim());
            if (request.RedeemPoints > card.Points)
                throw new ArgumentException("Недостатньо балів на картці лояльності.");

            var total = subtotal - request.RedeemPoints;
            // Earn 1 new point per 10 UAH actually paid.
            var earned = (int)decimal.Floor(total / 10m);
            var order = new FoodOrder
            {
                RestaurantId = request.RestaurantId,
                CustomerName = request.CustomerName.Trim(),
                Items = lines,
                Subtotal = subtotal,
                RedeemedPoints = request.RedeemPoints,
                EarnedPoints = earned
            };
            _orders.Add(order);
            card.Points = checked(card.Points - request.RedeemPoints + earned);
            return order;
        }
    }
}
