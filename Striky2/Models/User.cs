using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Striky.Api.Models;

public partial class User:IdentityUser
{


   public string Photo { get; set; } = string.Empty;

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public decimal? Height { get; set; }

    public decimal? Weight { get; set; }

    public string? FitnessLevel { get; set; }

    public string? Goal { get; set; }

    public DateTime? CreatedAt { get; set; }=DateTime.Now;

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
