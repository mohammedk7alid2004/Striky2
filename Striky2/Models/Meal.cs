using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Striky.Api.Models;

public class Meal
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "MealCategoryId is required.")]
    [ForeignKey("MealCategory")]
    public int MealCategoryId { get; set; }

    [Required(ErrorMessage = "Meal name is required.")]
    [MaxLength(100, ErrorMessage = "Meal name cannot exceed 100 characters.")]
    public string Name { get; set; }= string.Empty;

    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = string.Empty;

    [Range(0, 5000, ErrorMessage = "Calories must be between 0 and 5000.")]
    public int Calories { get; set; }

    public virtual MealCategory MealCategory { get; set; }
}