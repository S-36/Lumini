using Backend.DB.PostgreSQL;
using Backend.Error;
using Backend.src.User.dtos;
using Backend.src.User.Interface;
using Microsoft.EntityFrameworkCore;

namespace Backend.src.User.repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PostgreSQLContext _context;

        public UserRepository(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Result<User>> GetUserByEmailAsync(string email)
        {
            var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (result == null)
            {
                return Result<User>.Failure("This email is not registered", 404);
            }
            return Result<User>.Success(result);
        }

        public async Task<Result<User_Get>> GetUserByIdAsync(Guid userId)
        {
            var result = await _context.Users.FindAsync(userId);
            if (result == null)
            {
                return Result<User_Get>.Failure("User not found", 404);
            }
            return Result<User_Get>.Success(new User_Get
            {
                UserId = result.UserId,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email,
                UserRoles = result.UserRoles?.Count > 0 ? string.Join(",", result.UserRoles) : string.Empty,
                PhoneNumber = result.PhoneNumber,
                Address = result.Address
            });
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<User_Get>> GetAllUsersAsync(int pageNumber = 1, int pageSize = 10)
        {
            var users = await _context.Users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return users.Select(user => new User_Get
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserRoles = user.UserRoles?.Count > 0 ? string.Join(",", user.UserRoles) : string.Empty,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            });
        }
    }
}