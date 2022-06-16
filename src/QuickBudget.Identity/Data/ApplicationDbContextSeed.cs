using Microsoft.AspNetCore.Identity;
using QuickBudget.Domain.Entities;
using QuickBudget.Persistance;

namespace QuickBudget.Identity.Data
{
    public class ApplicationDbContextSeed
    {
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher = new PasswordHasher<ApplicationUser>();

        public async Task SeedAsync(ApplicationDbContext context, IWebHostEnvironment env,
            ILogger<ApplicationDbContextSeed> logger, int? retry = null)
        {
            int retryForAvaiability = retry ?? 0;

            try
            {
                var contentRootPath = env.ContentRootPath;
                var webroot = env.WebRootPath;

                if (!context.Users.Any())
                {
                    var user = new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = "validemail@email.com",
                        Email = "validemail@email.com",
                        Name = "Name",
                        LastName = "LastName",
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        NormalizedEmail = "VALIDEMAIL@EMAIL.COM",
                        NormalizedUserName = "VALIDEMAIL@EMAIL.COM",
                    };

                    user.PasswordHash = _passwordHasher.HashPassword(user, "PassSword123");
                    context.Users.Add(user);

                    await context.SaveChangesAsync();
                }


            }
            catch (Exception ex)
            {
                if (retryForAvaiability < 10)
                {
                    retryForAvaiability++;

                    logger.LogError(ex, "EXCEPTION ERROR while migrating {DbContextName}", nameof(ApplicationDbContext));

                    await SeedAsync(context, env, logger, retryForAvaiability);
                }
            }
        }
    }
}
