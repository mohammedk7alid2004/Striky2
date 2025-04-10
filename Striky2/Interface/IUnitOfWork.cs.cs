using  Striky.Api.Models;
using System.Data;
using static Striky.Api.Interface.IRepository;

namespace Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IRepository<Category> Categories { get; }
    public IRepository<ExerciseDetail> ExerciesDetail { get; }
    public IRepository<Meal> Meal { get; }
    public IRepository<MealCategory> MealCategory { get; }
    public IRepository<Notification> Notification { get; }
    public IRepository<Exercise> Exercy { get; }
    
    public IRepository<User> Users { get; }
    int Save();
}