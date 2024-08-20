using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

[Route("api/restaurant")]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        var restaurantDtos = _restaurantService.GetAll();
        return Ok(restaurantDtos);
    }

    [HttpGet("{id}")]
    public ActionResult<RestaurantDto> GetById([FromRoute] int id)
    {
        var restaurant = _restaurantService.GetById(id);

        if (restaurant is null)
        {
            return NotFound();
        }

        return Ok(restaurant);
    }

    [HttpPost]
    public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var id = _restaurantService.Create(dto);

        return Created($"/api/restaurant/{id}", null);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] int id)
    {
        bool isDeleted = _restaurantService.Delete(id);

        if (isDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPut("{id}")]
    public ActionResult Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var isUpdated = _restaurantService.Update(id, dto);

        if (!isUpdated)
        {
            return NotFound();
        }

        return Ok();
    }
}
