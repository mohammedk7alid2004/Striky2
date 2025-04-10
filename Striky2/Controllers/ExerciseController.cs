using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Striky2.Dto.Request;
using Striky2.Services.ExerciesServcies;

namespace Striky2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExerciseController : ControllerBase
{
    private readonly IExerciesServices _exerciesServices;
    private readonly IUnitOfWork _unitOfWork;
    public ExerciseController(IExerciesServices exerciesServices, IUnitOfWork unitOfWork)
    {
        _exerciesServices = exerciesServices;
        _unitOfWork = unitOfWork;
    }
    [HttpPost("Add")]
    public async Task <IActionResult> Add(ExerciseRequestDto exerciseRequest)
    {
        if(exerciseRequest == null)
        {
            return BadRequest("Exercise  is null");
        }
        await _exerciesServices.Create(exerciseRequest);
        return Ok("Exercise Created");
    }
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var exercises = await _exerciesServices.GetAll();
        if (exercises == null || !exercises.Any())
        {
            return NotFound("No exercises found");
        }
        return Ok(exercises);
    }
    [HttpGet("GetById/{id}")]
    public async Task <IActionResult> GetById( [FromRoute]int id)
    {
        var exercise = await _exerciesServices.GetById(id);
        if (exercise == null)
        {
            return NotFound("Exercise not found");
        }
        return Ok(exercise);
    }
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var exercise = await _exerciesServices.Delete(id);
        if (!exercise )
        {
            return NotFound("Exercise not found");
        }
        return Ok("Exercise Deleted");
    }
    [HttpPut("Update")]
    public async Task<IActionResult>Update(int id, ExerciseRequestDto exerciseRequest)
    {
        bool isupdated= await _exerciesServices.Update(id, exerciseRequest);
        return isupdated ? Ok("Done Updated") : NotFound();
    }
}
