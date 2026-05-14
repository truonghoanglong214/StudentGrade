using Microsoft.Extensions.DependencyInjection;
using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Application.Interfaces.IServices;
using StudentGrade.Application.Services;
using StudentGrade.Infrastructure.Repositories;
using StudentGrade.API.Helpers;
using StudentGrade.Application.Mappings;

namespace StudentGrade.API.Configurations
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // Services
            services.AddScoped<IAuthService, AuthService>();

            // Helpers
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserHelper, CurrentUserHelper>();

            // AutoMapper
            services.AddAutoMapper(cfg => 
            {
                cfg.AddProfile<UserProfile>();
            });

            return services;
        }
    }
}
