using Microsoft.EntityFrameworkCore;
using OnlineJudger.Domain.Entities;
using OnlineJudger.Domain.Stores;

namespace OnlineJudger.Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        private readonly OnlineJudgeContext _context;

        public UserRepository(OnlineJudgeContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IReadOnlyList<User>> GetAllOrderByPointsDesc()
        {
            return await _context.Users.OrderByDescending(u => u.Points).ToListAsync();
        }
    }
}
