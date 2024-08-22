using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

[Route("api/restaurant")]
[ApiController]
[Authorize]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService;
    }

    [HttpGet]
    [Authorize(Policy = "HasNationality")]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery] RestaurantQuery query)
    {
        var restaurantDtos = _restaurantService.GetAll(query);
        return Ok(restaurantDtos);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public ActionResult<RestaurantDto> GetById([FromRoute] int id)
    {
        var restaurant = _restaurantService.GetById(id);

        return Ok(restaurant);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
    {
        var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var id = _restaurantService.Create(dto);

        return Created($"/api/restaurant/{id}", null);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult Delete([FromRoute] int id)
    {
        _restaurantService.Delete(id);

        return NoContent();
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public ActionResult Update([FromBody] UpdateRestaurantDto dto, [FromRoute] int id)
    {
        _restaurantService.Update(id, dto);

        return Ok();
    }
}
