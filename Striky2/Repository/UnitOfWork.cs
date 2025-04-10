

namespace Striky.Api.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly Db16821Context _context;

        // Repositories
        public IRepository<Category> Categories { get; }
        public IRepository<ExerciseDetail> ExerciesDetail { get; }
        public IRepository<Meal> Meal { get; }
        public IRepository<MealCategory> MealCategory { get; }
        public IRepository<Notification> Notification { get; }
        public IRepository<Exercise> Exercy { get; }
       
        public IRepository<User> Users { get; }

        public UnitOfWork(Db16821Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            // Initialize all repositories
            Categories = new Repository<Category>(_context);
            ExerciesDetail = new Repository<ExerciseDetail>(_context);
            Meal = new Repository<Meal>(_context);
            MealCategory = new Repository<MealCategory>(_context);
            Notification = new Repository<Notification>(_context);
            Exercy = new Repository<Exercise>(_context);
       
            Users = new Repository<User>(_context);
        }

        public int Save()
        {
            
                return _context.SaveChanges();
            
           
        }

        #region IDisposable Implementation
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}