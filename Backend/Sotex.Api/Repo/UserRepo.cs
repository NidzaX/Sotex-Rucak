using Sotex.Api.Interfaces;

namespace Sotex.Api.Repo
{
    public class UserRepo
    {
        private ProjectDbContext projectDbContext;

        public UserRepo(ProjectDbContext context)
        {
            projectDbContext = context;
        }
    }
}
