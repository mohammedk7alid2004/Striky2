using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Striky.Api.Models;

public partial class ExerciseDetail
{
    [Key]
    public int ExerciseDetailId { get; set; }   
    public string? Name { get; set; }

    public string? Photo { get; set; }

    public string? Description { get; set; }

    public int? Count { get; set; }

    public int? Douration { get; set; }

    public string? Vedo { get; set; }
    [ForeignKey("ExerciseId")]
    public int? ExerciseId { get; set; }
    public virtual Exercise Exercise{ get; set; }

}
