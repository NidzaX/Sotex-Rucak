using Sotex.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Sotex.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            using var dbContext = scope.ServiceProvider.GetRequiredService<ProjectDbContext>();

            dbContext.Database.Migrate();
        }
    }
}