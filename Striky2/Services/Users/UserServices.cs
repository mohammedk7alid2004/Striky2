using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Striky.Api.Models;
using Striky2.Dto.Request;
using Striky2.Dto.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Striky2.Services.Users
{
    public class UserServices : IUserServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;
        private readonly string _imagePath;
        private readonly UserManager<User> _userManager;

        public UserServices(IUnitOfWork unitOfWork, IWebHostEnvironment env, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _userManager = userManager;

            _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Images");
            Directory.CreateDirectory(_imagePath);
        }

        public async Task<bool> Create(UserRequest f)
        {
            if (f == null)
                throw new ArgumentNullException(nameof(f));

            string? fileName = null;

            if (f.Photo != null && f.Photo.Length > 0)
            {
                fileName = $"{Guid.NewGuid()}{Path.GetExtension(f.Photo.FileName)}";
                var fullPath = Path.Combine(_imagePath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await f.Photo.CopyToAsync(stream);
                }
            }

            var user = new User
            {
                UserName = $"{f.FirstName}{f.LastName}",
                FitnessLevel = f.FitnessLevel,
                Goal = f.Goal,
                Age = f.Age,
                Height = f.Height,
                Weight = f.Weight,
                Email = f.Email,
                CreatedAt = DateTime.Now,
                Gender = f.Gender,
                PhoneNumber = f.phone,
                Photo = fileName
            };

            var result = await _userManager.CreateAsync(user, f.Password);

            if (!result.Succeeded)
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            return true;
        }

        public async Task<string?> Login(LoginDto loginDto)
        {
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto));

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key_here"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7234",
                audience: "https://localhost:4200",
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public Task<IEnumerable<UserResponse>> GetAll()
        {
            var users = _unitOfWork.Users.GetAll().ToList();

            var result = users.Select(u => new UserResponse
            {
                FirstName = u.UserName ?? string.Empty,
                LastName = string.Empty,
                Email = u.Email ?? string.Empty,
                Age = u.Age,
                Height = u.Height.HasValue ? (int)u.Height.Value : 0,
                Weight = u.Weight,
                FitnessLevel = u.FitnessLevel,
                Goal = u.Goal,
                CreatedAt = u.CreatedAt,
              
            });

            return Task.FromResult(result);
        }

        public  Task<UserResponse> GetById(int id)
        {
            var user =  _unitOfWork.Users.GetById(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            var result = new UserResponse
            {
                FirstName = user.UserName,
                LastName = "",
                Email = user.Email,
                Age = user.Age,
                Height = user.Height.HasValue ? (int)user.Height.Value : 0,
                Weight = user.Weight,
                FitnessLevel = user.FitnessLevel,
                Goal = user.Goal,
                CreatedAt = user.CreatedAt,
                Photo = GetFullImagePath(user.Photo)
            };

            return Task.FromResult(result);
        }

        public async Task<bool> Update(int id, UserRequest f)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            user.UserName = $"{f.FirstName}{f.LastName}";
            user.FitnessLevel = f.FitnessLevel;
            user.Goal = f.Goal;
            user.Age = f.Age;
            user.Height = f.Height;
            user.Weight = f.Weight;
            user.Email = f.Email;
            user.Gender = f.Gender;
            user.PhoneNumber = f.phone;

            if (f.Photo != null && f.Photo.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(f.Photo.FileName)}";
                var fullPath = Path.Combine(_imagePath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await f.Photo.CopyToAsync(stream);
                }

                user.Photo = fileName;
            }

            _unitOfWork.Users.Update(user);
            _unitOfWork.Save();

            return true;
        }
        public async Task<bool> Delete(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            _unitOfWork.Users.Delete(user);
             _unitOfWork.Save();

            return true;
        }

        private string GetFullImagePath(string photoFileName)
        {
            if (string.IsNullOrEmpty(photoFileName))
                return string.Empty;

            var fullPath = Path.Combine(_imagePath, photoFileName);
            if (!File.Exists(fullPath))
                return string.Empty;

            return $"/UploadedFiles/Images/{photoFileName}";
        }

    }
}
