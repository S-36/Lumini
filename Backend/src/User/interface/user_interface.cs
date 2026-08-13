using Backend.Error;
using Backend.src.User.dtos;

namespace Backend.src.User.Interface
{
    public interface IUserRepository
    {
        Task<User> RegisterUserAsync(User user);
        Task<Result<User_Get>> GetUserByEmailAsync(string email);
        Task<Result<User_Get>> GetUserByIdAsync(Guid userId);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<IEnumerable<User_Get>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10);

    }

    public interface IUserService
    {
        Task<User> RegisterUserAsync(User_Register_DTO userDto);
        Task<User> LoginUserAsync(User_Login_DTO loginDto);

    }
}