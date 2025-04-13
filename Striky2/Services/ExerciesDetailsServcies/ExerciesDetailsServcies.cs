using Microsoft.Extensions.Options;
using Striky.Api.Models;
using Striky2.Dto.Request;
using Striky2.Dto.Response;

using Striky2.Models;

namespace Striky2.Services.ExerciesDetailsServcies
{
    public class ExerciesDetailsServcies : IExerciesDetailsServcies
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly string VedoPath;

        public ExerciesDetailsServcies(IUnitOfWork unitOfWork, IWebHostEnvironment env, IOptions<AppSettings> options)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            VedoPath = Path.Combine(Directory.GetCurrentDirectory(), options.Value.VedoPath);
            Directory.CreateDirectory(VedoPath);
        }
        

        public async Task Create(ExerciesDetailsDto exercise)
        {
           
            if (exercise.VideoData == null || exercise.VideoData.Length == 0)
                throw new ArgumentException("No video file provided.");

            var videoName = $"{Guid.NewGuid()}.mp4"; 
            var fullPath = Path.Combine(VedoPath, videoName);

            await File.WriteAllBytesAsync(fullPath, exercise.VideoData);

            var exerciseDetail = new ExerciseDetail()
            {
                Name = exercise.Name,
                Description = exercise.Description,
                Count = exercise.Count,
                Douration = exercise.Douration,
                ExerciseId = exercise.ExerciseId,
                Vedo = videoName
            };

            await _unitOfWork.ExerciesDetail.AddAsync(exerciseDetail);
            _unitOfWork.Save();
        }
        public Task<bool> Delete(int id)
        {
            var  ed = _unitOfWork.ExerciesDetail.GetById(id);
            if (ed == null)
            {
                throw new KeyNotFoundException("Exercise detail not found.");
            }
            _unitOfWork.ExerciesDetail.Delete(ed);
            _unitOfWork.Save();
            var vedopath = Path.Combine(VedoPath, ed.Vedo);
            if (File.Exists(vedopath))
            {
                File.Delete(vedopath);
            }
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<ExerciesDetailsResponse>> GetAll()
        {
            var ed = _unitOfWork.ExerciesDetail.GetAll().ToList();
            var response = ed.Select(e => new ExerciesDetailsResponse
            {
                ExerciseDetailId = e.ExerciseDetailId,
                Name = e.Name ?? string.Empty,
                Description = e.Description ?? string.Empty,
                Count = e.Count ?? 0,
                Douration = e.Douration ?? 0,
                Vedo = e.Vedo ?? string.Empty,
                ExerciseId = e.ExerciseId
            }).ToList();
            return await Task.FromResult(response);
        }

        public async Task<ExerciesDetailsResponse> GetById(int id)
        {
            var exerciseDetail = await _unitOfWork.ExerciesDetail.GetByIdAsync(id);
            if (exerciseDetail == null)
            {
                throw new KeyNotFoundException("Exercise detail not found.");
            }

            var response = new ExerciesDetailsResponse
            {
                ExerciseDetailId = exerciseDetail.ExerciseDetailId,
                Name = exerciseDetail.Name ?? string.Empty,
                Description = exerciseDetail.Description ?? string.Empty,
                Count = exerciseDetail.Count ?? 0,
                Douration = exerciseDetail.Douration ?? 0,
                Vedo = exerciseDetail.Vedo ?? string.Empty,
                ExerciseId = exerciseDetail.ExerciseId
            };

            return response;
        }

        public  async Task<bool> Update(int id, ExerciesDetailsDto exercise)
        {
            var exerciseDetail = _unitOfWork.ExerciesDetail.GetById(id);
            if (exerciseDetail == null)
            {
                throw new KeyNotFoundException("Exercise detail not found.");
            }
            exerciseDetail.Name = exercise.Name;
            exerciseDetail.Description = exercise.Description;
            exerciseDetail.Count = exercise.Count;
            exerciseDetail.Douration = exercise.Douration;
            exerciseDetail.ExerciseId = exercise.ExerciseId;
            if (exercise.VideoData != null && exercise.VideoData.Length > 0)
            {
                var videoName = $"{Guid.NewGuid()}.mp4";
                var fullPath = Path.Combine(VedoPath, videoName);
                await File.WriteAllBytesAsync(fullPath, exercise.VideoData);
                exerciseDetail.Vedo = videoName;
            }
            _unitOfWork.ExerciesDetail.Update(exerciseDetail);
            _unitOfWork.Save();
            return (true);
        }
    }
}
