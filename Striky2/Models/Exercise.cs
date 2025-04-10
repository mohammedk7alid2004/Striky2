using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Striky.Api.Models;

public class Exercise
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "CategoryId is required.")]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Exercise name is required.")]
    [MaxLength(100, ErrorMessage = "Exercise name cannot exceed 100 characters.")]
    public string Name { get; set; }

   

    [Url(ErrorMessage = "Photo must be a valid URL.")]
    public string Photo { get; set; }

    [Range(1, 1000, ErrorMessage = "Count must be between 1 and 1000.")]
    public int? Count { get; set; }

    [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes.")]
    public int? Duration { get; set; }

    public virtual Category Category { get; set; }
}