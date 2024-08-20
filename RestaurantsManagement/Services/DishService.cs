using AutoMapper;
using Microsoft.EntityFrameworkCore;

public interface IDishService
{
    int Create(int restaurantId, CreateDishDto dto);
    DishDto GetById(int restaurantId, int dishId);
    List<DishDto> GetAll(int restaurantId);
    public void RemoveAll(int restaurantId);
    void RemoveById(int restaurantId, int dishId);
}

public class DishService : IDishService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IMapper _mapper;

    public DishService(RestaurantDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public int Create(int restaurantId, CreateDishDto dto)
    {
        var restaurant = GetRestaurantById(restaurantId);

        var dishEntity = _mapper.Map<Dish>(dto);

        dishEntity.RestaurantId = restaurantId;
        _dbContext.Dishes.Add(dishEntity);
        _dbContext.SaveChanges();

        return dishEntity.Id;
    }

    public DishDto GetById(int restaurantId, int dishId)
    {
        var restaurant = GetRestaurantById(restaurantId);

        var dish = GetDishById(restaurantId, dishId, restaurant);

        var DishDto = _mapper.Map<DishDto>(dish);

        return DishDto;
    }

    public List<DishDto> GetAll(int restaurantId)
    {
        var restaurant = _dbContext
            .Restaurants.Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == restaurantId);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var DishDtos = _mapper.Map<List<DishDto>>(restaurant.Dishes);

        return DishDtos;
    }

    public void RemoveAll(int restaurantId)
    {
        var restaurant = GetRestaurantById(restaurantId);

        _dbContext.RemoveRange(restaurant.Dishes);
        _dbContext.SaveChanges();
    }

    public void RemoveById(int restaurantId, int dishId)
    {
        var restaurant = GetRestaurantById(restaurantId);
        var dish = GetDishById(restaurantId, dishId, restaurant);

        _dbContext.Remove(dish);
        _dbContext.SaveChanges();
    }

    private Restaurant GetRestaurantById(int restaurantId)
    {
        var restaurant = _dbContext
            .Restaurants.Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == restaurantId);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        return restaurant;
    }

    private Dish GetDishById(int restaurantId, int dishId, Restaurant restaurant)
    {
        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);

        if (dish is null || dish.RestaurantId != restaurantId)
        {
            throw new NotFoundException("Dish not found");
        }

        return dish;
    }
}
