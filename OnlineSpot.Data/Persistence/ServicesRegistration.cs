using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineSpot.Data.Domain.Interfaces;
using OnlineSpot.Data.Persistence.Context;
using OnlineSpot.Data.Persistence.Repositories;


namespace OnlineSpot.Data.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<OnlineSpotDbContext>(options => options.UseInMemoryDatabase("ApplicationDb"));
            }
            else
            {
                services.AddDbContext<OnlineSpotDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                m => m.MigrationsAssembly(typeof(OnlineSpotDbContext).Assembly.FullName)));
            }
            #endregion

            #region Repositories
            services.AddTransient<IUserRepository, UserRepository>();


            #endregion
        }
    }
}
