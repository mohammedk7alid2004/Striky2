using Striky.Api.Dto;
using Striky2.Dto;

namespace Striky.Api.Services;

public interface ICategoryServices
{
    Task Create (CategoryDto categoryDto);
    Task <IEnumerable<CategoryResponseDto>> GetAll ();
    Task<CategoryResponseDto> GetById(int id);
    Task <bool> Update(int id, CategoryDto categoryDto);

}
