using Microsoft.AspNetCore.Identity.Data;
using Striky2.Dto.Request;
using Striky2.Dto.Response;

namespace Striky2.Services.Users
{
    public interface IUserServices
    {
        public Task<IEnumerable<UserResponse>> GetAll();
        public  Task<bool>   Create(UserRequest f);
        public  Task<string> Login(LoginDto loginDto);
        public Task<bool> Update(int id , UserRequest f);
        public Task<bool> Delete(int id);
        public Task<UserResponse> GetById(int id);
    }
}
