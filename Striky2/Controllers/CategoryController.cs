using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Striky.Api.Dto;
using Striky.Api.Services;

namespace Striky.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(IUnitOfWork unitOfWork, CategoryServices categoryServices) : ControllerBase
{
    private IUnitOfWork UnitOfWork { get; } = unitOfWork;
    public CategoryServices CategoryServices { get; } = categoryServices;

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll()
    {
        var categories = await CategoryServices.GetAll();
        return Ok(categories);
    }
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var category = await CategoryServices.GetById(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }
    [HttpPost("Create")]
    public async Task<IActionResult> Create(CategoryDto category)
    {
        if (category == null)
        {
            return BadRequest();
        }
        await CategoryServices.Create(category);
        UnitOfWork.Save();
        return Ok("Done Created");
    }
    [HttpDelete("Remove")]
    public async Task<IActionResult> Remove(int id)
    {
        var category = await UnitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        UnitOfWork.Categories.Delete(category);
        UnitOfWork.Save();
        return Ok("Done Deleted");
    }
    [HttpPut("Update")]
    public async Task<IActionResult> Update(int id, CategoryDto category)
    {
       bool IsUpdate=  await  CategoryServices.Update(id, category);
        return IsUpdate ? Ok("Done Updated") : NotFound();

    }

}