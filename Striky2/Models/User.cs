using System;
using System.Collections.Generic;

namespace Striky.Api.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public string? FitnessLevel { get; set; }

    public string? Goal { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
