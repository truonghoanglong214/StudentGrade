using Microsoft.Extensions.DependencyInjection;
using StudentGrade.Application.Interfaces.IRepositories;
using StudentGrade.Application.Interfaces.IServices;
using StudentGrade.Application.Services;
using StudentGrade.Infrastructure.Repositories;
using StudentGrade.Infrastructure.Services;
using StudentGrade.Infrastructure.Services.Excel;
using StudentGrade.Infrastructure.Services.FG;
using StudentGrade.Infrastructure.Tenancy;
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
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentScoreRepository, StudentScoreRepository>();
            services.AddScoped<IAssessmentRepository, AssessmentRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<IImportHistoryRepository, ImportHistoryRepository>();
            services.AddScoped<IFgExportRepository, FgExportRepository>();
            services.AddScoped<ITransaction, Transaction>();

            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IExcelImportService, EPPlusExcelImportService>();
            services.AddScoped<IFGSerializer, BinaryFGSerializer>();
            services.AddScoped<IGradeService, GradeService>();

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
