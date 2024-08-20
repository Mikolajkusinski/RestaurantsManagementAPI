using Microsoft.AspNetCore.Mvc;

[Route("api/restaurant/{restaurantId}/dish")]
[ApiController]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;
    }

    [HttpPost]
    public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
    {
        var newDishId = _dishService.Create(restaurantId, dto);

        return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
    }

    [HttpGet("{dishId}")]
    public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dish = _dishService.GetById(restaurantId, dishId);

        return Ok(dish);
    }

    [HttpGet]
    public ActionResult<List<DishDto>> Get([FromRoute] int restaurantId)
    {
        var results = _dishService.GetAll(restaurantId);

        return Ok(results);
    }

    [HttpDelete]
    public ActionResult Delete([FromRoute] int restaurantId)
    {
        _dishService.RemoveAll(restaurantId);
        return NoContent();
    }

    [HttpDelete("{dishId}")]
    public ActionResult Delete([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        _dishService.RemoveById(restaurantId, dishId);
        return NoContent();
    }
}
