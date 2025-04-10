using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Striky.Api.Models;

namespace Striky.Api.Data;

public partial class Db16821Context : DbContext
{


    public Db16821Context(DbContextOptions<Db16821Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ExerciseDetail> ExerciesDetails { get; set; }

    public virtual DbSet<Exercise> Exercies { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<MealCategory> MealCategories { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Height)
            .HasPrecision(5, 2);

        modelBuilder.Entity<User>()
            .Property(u => u.Weight)
            .HasPrecision(5, 2);
    }


   
}
