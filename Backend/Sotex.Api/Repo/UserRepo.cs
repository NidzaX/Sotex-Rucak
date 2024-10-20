using Microsoft.EntityFrameworkCore;
using Sotex.Api.Infrastructure;
using Sotex.Api.Model;

namespace Sotex.Api.Repo
{
    public class UserRepo
    {
        private readonly ProjectDbContext _projectDbContext;

        public UserRepo(ProjectDbContext context)
        {
            _projectDbContext = context;
        }

        public async Task<User> AddUserAsync(User user)
        {
            var retVal = await _projectDbContext.Users.AddAsync(user);
            await _projectDbContext.SaveChangesAsync();
            return retVal.Entity;
        }

        public async Task<User> FindUserByUsernameAsync(string username)
        {
            return await _projectDbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return await _projectDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User> FindAsync(Guid id)
        {
            return await _projectDbContext.Users.FindAsync(id);
        }
    }
}
