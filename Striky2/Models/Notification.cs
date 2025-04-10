using System;
using System.Collections.Generic;

namespace Striky.Api.Models;

public partial class Notification
{
    public int Id { get; set; }

    public string? Message { get; set; }

    public int? UserId { get; set; }

    public DateTime? Date { get; set; }

    public virtual User? User { get; set; }
}
