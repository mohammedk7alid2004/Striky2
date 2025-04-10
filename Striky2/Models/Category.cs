using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Striky.Api.Models;

public partial class Category
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    public string Name { get; set; }

    [Url(ErrorMessage = "Photo must be a valid URL.")]
    public string Photo { get; set; }

    public int CountExercises { get; set; }

    public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();
}
