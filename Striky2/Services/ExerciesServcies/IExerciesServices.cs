using Striky2.Dto.Request;
using Striky2.Dto.Response;

namespace Striky2.Services.ExerciesServcies
{
    public interface IExerciesServices
    {
        public Task<IEnumerable<Exercies_Response>> GetAll();
        public Task<Exercies_Response> GetById(int id );
        public Task Create(ExerciseRequestDto exercise );
        public Task<bool> Update(int id, ExerciseRequestDto exercise);
        public Task<bool> Delete(int id);
    }
}
