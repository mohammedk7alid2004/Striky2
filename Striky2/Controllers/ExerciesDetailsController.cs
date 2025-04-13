
using Striky2.Dto.Request;
using Striky2.Services.ExerciesDetailsServcies;

namespace Striky2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExerciesDetailsController : ControllerBase
{
    private readonly IExerciesDetailsServcies _exerciesDetailsServcies;



    public ExerciesDetailsController(IExerciesDetailsServcies exerciesDetailsServcies, IUnitOfWork unitOfWork)
    {
        _exerciesDetailsServcies = exerciesDetailsServcies;
    }
    [HttpPost("Add")]
    public async Task<IActionResult> Create([FromForm] ExerciesDetailsDto exercise)
    {
        await _exerciesDetailsServcies.Create(exercise);
        return Ok();
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _exerciesDetailsServcies.GetAll();
        return Ok(result);
    }
    [HttpGet("{Id}")]
    public async Task<IActionResult> GetById(int Id)
    {
        var result = await _exerciesDetailsServcies.GetById(Id);
        return Ok(result);
    }
    [HttpPut("update")]
    public async Task<IActionResult> Update(ExerciesDetailsDto exerciseRequest, int id)
    {
        var result = await _exerciesDetailsServcies.Update(id, exerciseRequest);
        if (result)
        {
            return Ok();
        }
        return NotFound();
    }
    [HttpDelete("Delete")]
    public IActionResult Delete(int id)
    {
        var IsDeleted = _exerciesDetailsServcies.Delete(id);
        if (IsDeleted.Result)
        {
            return Ok();
        }
        return NotFound();

    }
}
