using Striky2.Dto.Request;
using Striky2.Dto.Response;

namespace Striky2.Services.ExerciesDetailsServcies;

public interface IExerciesDetailsServcies

{
    public Task<IEnumerable<ExerciesDetailsResponse>> GetAll();
    public Task<ExerciesDetailsResponse> GetById(int id);
    public Task Create(ExerciesDetailsDto exercise);
    public Task<bool> Update(int id, ExerciesDetailsDto exercise);
    public Task<bool> Delete(int id);

}
