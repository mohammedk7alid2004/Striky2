using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Striky.Api.Dto;
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
        private readonly UserManager<User> userManager;

        public UserServices(IUnitOfWork unitOfWork, IWebHostEnvironment env, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _env = env;
            _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", "Images");
            Directory.CreateDirectory(_imagePath);
            Directory.CreateDirectory(_imagePath);
            this.userManager = userManager;
        }

        public async Task<bool> Create(UserRequest f)
        {
            if (f.Photo == null || f.Photo.Length == 0)
                throw new ArgumentException("No photo uploaded");

            var covername = $"{Guid.NewGuid()}{Path.GetExtension(f.Photo.FileName)}";
            var fullPath = Path.Combine(_imagePath, covername);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await f.Photo.CopyToAsync(stream);
            }

            if (f == null)
                throw new ArgumentNullException(nameof(f), "User request cannot be null.");
            User s = new()
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
                Photo = covername,

            };
            var result = await userManager.CreateAsync(s, f.Password);
            if (!result.Succeeded)
            {
                throw new Exception("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            return true;
        }

        public Task<IEnumerable<UserResponse>> GetAll()
        {
            var users = _unitOfWork.Users.GetAll().ToList();
            var userResponses = users.Select(u => new UserResponse
            {
                FirstName = u.UserName,
                LastName = "", 
                Email = u.Email,
                Age = u.Age,
                Height = u.Height.HasValue ? (int)u.Height.Value : 0, // Handle null height
                Weight = u.Weight,
                FitnessLevel = u.FitnessLevel,
                Goal = u.Goal,
                CreatedAt = u.CreatedAt,
            }).ToList();

            return Task.FromResult(userResponses.AsEnumerable());
        }
        public async Task<bool> Login(LoginDto loginDto)
        {
            if (loginDto == null)
            {
                throw new ArgumentNullException(nameof(loginDto));
            }

            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return  false;
            }

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
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return await userManager.CheckPasswordAsync(user, loginDto.Password);

        }
        private string GetFullImagePath(string photoFileName)
        {
            if (string.IsNullOrEmpty(photoFileName))
                return string.Empty;

            return $"/Contracts/asset/image/{photoFileName}";
        }
    }
}


