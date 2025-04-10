using System;
using System.Collections.Generic;

namespace Striky.Api.Models;

public partial class MealCategory
{
    public int MealCategoryId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();
}
