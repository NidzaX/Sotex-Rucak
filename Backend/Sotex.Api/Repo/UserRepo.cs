
using Sotex.Api.Infrastructure;
using Sotex.Api.Model;

namespace Sotex.Api.Repo
{
    public class UserRepo
    {
        private ProjectDbContext projectDbContext;

        public UserRepo(ProjectDbContext context)
        {
            projectDbContext = context;
        }

        public User AddUser(User user)
        {
            User retVal = projectDbContext.Users.Add(user).Entity;
            projectDbContext.SaveChanges();
            return retVal;
        }

        public User FindUserByUsername(string username)
        {
            return projectDbContext.Users.FirstOrDefault(x => x.Username == username);
        }
        
        public User FindByEmail(string email)
        {
            return projectDbContext.Users.FirstOrDefault(x => x.Email == email);
        }

        public User Find(Guid id)
        {
            return projectDbContext.Users.Find(id);
        }
    }
}
