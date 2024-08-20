using System.ComponentModel.DataAnnotations;

public class UpdateRestaurantDto
{
    [Required]
    [MaxLength(25)]
    public string Name { get; set; }
    public string Description { get; set; }
    public bool HasDelivery { get; set; }
}
