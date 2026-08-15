using Backend.Error;
using Backend.JWT;
using Backend.src.User.dtos;
using Backend.src.User.Interface;

namespace Backend.src.User.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public UserService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<Result<User>> RegisterUserAsync(User_Register_DTO userDto)
        {
            // Map the DTO to the User entity
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = userDto.Email,
                PasswordHash = HashPassword(userDto.Password),
                UserRoles = string.IsNullOrWhiteSpace(userDto.UserRoles)
                    ? []
                    : [userDto.UserRoles]
            };

            var registeredUser = await _userRepository.RegisterUserAsync(user);
            return Result<User>.Success(registeredUser);
        }

        public async Task<Result<string>> LoginUserAsync(User_Login_DTO loginDto)
        {
            var result = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (!result.IsSuccess || result.Value == null)
            {
                return Result<string>.Failure("Invalid email or password.", 401);
            }

            var user = result.Value;
            if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            {
                return Result<string>.Failure("Invalid email or password.", 401);
            }

            // Generate JWT token and return it
            var token = GenerateJwtToken(user);
            return Result<string>.Success(token);
        }

        private string HashPassword(string password)
        {
            // Implement password hashing logic here
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            // Implement password verification logic here
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        private string GenerateJwtToken(User user)
        {
            return _jwtService.GenerateToken(user.UserId.ToString(), user.Email, user.UserRoles);
        }
    }
}