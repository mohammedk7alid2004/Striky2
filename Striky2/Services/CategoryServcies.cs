using Striky.Api.Dto;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System;
using Striky2.Dto;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Striky.Api.Services
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly string _imagePath;

        // Constructor
        public CategoryServices(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _env = webHostEnvironment;

            _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Images");

            Directory.CreateDirectory(_imagePath);
        }

        // Create Category with file upload
        public async Task Create(CategoryDto categoryDto)
        {
            if (categoryDto.Photo == null || categoryDto.Photo.Length == 0)
                throw new ArgumentException("No photo uploaded");

            var covername = $"{Guid.NewGuid()}{Path.GetExtension(categoryDto.Photo.FileName)}";
            var fullPath = Path.Combine(_imagePath, covername);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await categoryDto.Photo.CopyToAsync(stream);
            }

            Category category = new()
            {
                Name = categoryDto.Name,
                Photo = covername,
                CountExercises = categoryDto.CountExercises
            };

            await _unitOfWork.Categories.AddAsync(category);
               _unitOfWork.Save();
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAll()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();

                return categories.Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    PhotoUrl = GetFullImagePath(c.Photo),
                    CountExercises = c.CountExercises
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving categories", ex);
            }
        }

        public async Task<CategoryResponseDto> GetById(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (category == null)
            {
             
                return null;
            }

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                PhotoUrl = GetFullImagePath(category.Photo),
                CountExercises = category.CountExercises
            };
        }
        public async Task<bool> Update(int id, CategoryDto categoryDto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return false;
            }

            category.Name = categoryDto.Name;
            category.CountExercises = categoryDto.CountExercises;

            if (categoryDto.Photo != null && categoryDto.Photo.Length > 0)
            {
                var covername = $"{Guid.NewGuid()}{Path.GetExtension(categoryDto.Photo.FileName)}";
                var fullPath = Path.Combine(_imagePath, covername);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await categoryDto.Photo.CopyToAsync(stream);
                }

                category.Photo = GetFullImagePath(covername);
            }

             _unitOfWork.Categories.Update(category);
            _unitOfWork.Save();

            return true;
        }

        private string GetFullImagePath(string photoFileName)
        {
            if (string.IsNullOrEmpty(photoFileName))
                return null;

            return Path.Combine("/Contracts/asset/image", photoFileName).Replace("\\", "/");
        }
    }
}
