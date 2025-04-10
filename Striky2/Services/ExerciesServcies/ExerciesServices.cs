using Striky.Api.Dto;
using Striky.Api.Models;
using Striky2.Dto.Request;
using Striky2.Dto.Response;

namespace Striky2.Services.ExerciesServcies
{
    public class ExerciesServices : IExerciesServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly string _imagePath;
        public ExerciesServices(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _env = webHostEnvironment;
            _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Images");
            Directory.CreateDirectory(_imagePath);
        }
        public async Task Create(ExerciseRequestDto exercise)
        {
            var covername = $"{Guid.NewGuid()}{Path.GetExtension(exercise.Photo.FileName)}";
            var fullPath = Path.Combine(_imagePath, covername);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await exercise.Photo.CopyToAsync(stream);
            }
            Exercise exercise1 = new()
            {
                Name = exercise.Name,
                Photo = covername,
                Count = exercise.Count,
                Duration = exercise.Duration,
                CategoryId = exercise.CategoryId

            };
            await _unitOfWork.Exercy.AddAsync(exercise1);
            _unitOfWork.Save();

        }
        public async Task<IEnumerable<Exercies_Response>> GetAll()
        {
            var exercises = await _unitOfWork.Exercy.GetAllAsync();

            if (exercises == null || !exercises.Any())
            {
                return Enumerable.Empty<Exercies_Response>();
            }

            return exercises.Select(e => new Exercies_Response
            {
                Id = e.Id,
                CategoryId = e.CategoryId,
                Name = e.Name,
                Photo = GetFullImagePath(e.Photo),
                Count = e.Count,
                Duration = e.Duration
            });
        }

        private string GetFullImagePath(string photoFileName)
        {
            if (string.IsNullOrEmpty(photoFileName))
                return string.Empty;

            return $"/Contracts/asset/image/{photoFileName}";
        }

        public async Task<Exercies_Response> GetById(int id)
        {
            var exercise = await _unitOfWork.Exercy.GetByIdAsync(id)
                ;
            return new Exercies_Response
            {
                Id = exercise.Id,
                CategoryId = exercise.CategoryId,
                Name = exercise.Name,
                Photo = GetFullImagePath(exercise.Photo),
                Count = exercise.Count,
                Duration = exercise.Duration
            };
        }

        public async Task<bool> Update(int id, ExerciseRequestDto exerciseDto)
        {
            // 1. جلب التمرين الموجود
            var existingExercise = await _unitOfWork.Exercy.GetByIdAsync(id);
            if (existingExercise == null)
            {
                return false;
            }

            // 2. تحديث الخصائص الأساسية
            existingExercise.Name = exerciseDto.Name;
            existingExercise.CategoryId = exerciseDto.CategoryId;
            existingExercise.Count = exerciseDto.Count;
            existingExercise.Duration = exerciseDto.Duration;

            if (exerciseDto.Photo != null && exerciseDto.Photo.Length > 0)
            {
                if (!string.IsNullOrEmpty(existingExercise.Photo))
                {
                    var oldImagePath = Path.Combine(_imagePath, Path.GetFileName(existingExercise.Photo));
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }
                }

                var covername = $"{Guid.NewGuid()}{Path.GetExtension(exerciseDto.Photo.FileName)}";
                var fullPath = Path.Combine(_imagePath, covername);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await exerciseDto.Photo.CopyToAsync(stream);
                }

                existingExercise.Photo = covername;
            }

            _unitOfWork.Exercy.Update(existingExercise);
             _unitOfWork.Save();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var exercise =  await _unitOfWork.Exercy.GetByIdAsync(id);
            if (exercise == null)
            {
                throw new Exception("Exercise not found");
            }
            _unitOfWork.Exercy.Delete(exercise);
            _unitOfWork.Save();
            return true;
        }
    }
}
