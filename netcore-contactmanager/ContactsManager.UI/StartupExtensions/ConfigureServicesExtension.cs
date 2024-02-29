using ContactManager.Core.Domain.IdentityEntites;
using CRUDexample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace CRUDexample.StartupExtensions
{
    public static class ConfigureServicesExtension
    {
        //Este metodo extiende a IServiceCollection
        public static IServiceCollection ConfigureServices(this IServiceCollection services
                        , IConfiguration configuration)
        {
            //Agregamos los action filters
            services.AddTransient<ResponseHeaderActionFilter>();

            services.AddControllersWithViews(
                options =>
                {
                    var logger = services.BuildServiceProvider()
                                .GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

                    //options.Filters.Add(new ResponseHeaderActionFilter(logger, "X-Custom-Global-Key", "Custom-Custom-Global-Value", 2));
                    //options.Filters.Add(new ResponseHeaderActionFilter("X-Custom-Global-Key", "Custom-Custom-Global-Value", 2));

                    options.Filters.Add(new ResponseHeaderActionFilter(logger)
                    {
                        Key = "X-Custom-Global-Key",
                        Value = "Custom-Custom-Global-Value",
                        Order = 2
                    });

                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                });

            //Add Services to IoC container
            services.AddScoped<ICountriesService, CountriesService>();
            services.AddScoped<IPersonsAdderService, PersonsAdderService>();

            //services.AddScoped<IPersonsGetterService, PersonsGetterServiceChild>();
            services.AddScoped<IPersonsGetterService, PersonsGetterServiceWithFewExcelFields>();
            services.AddScoped<PersonsGetterService, PersonsGetterService>();

            services.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
            services.AddScoped<IPersonsSorterService, PersonsSorterService>();
            services.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();

            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddScoped<IPersonsRepository, PersonsRepository>();

            services.AddTransient<PersonsListActionFilter>();

            //Scoped Service as Default
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            #region Enable identity

            services.AddIdentity<ApplicationUser, ApplicationRole>(
                options =>
                {
                    options.Password.RequiredLength = 5;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredUniqueChars = 3;
                    options.User.RequireUniqueEmail = true;
                    //options.SignIn.RequireConfirmedEmail = true;
                }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>()

                .AddDefaultTokenProviders()

                .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
                .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

            services.AddAuthorization(
                options =>
                {
                    //Enforces authorization by default.
                    //User must be authenticated for all action methods
                    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

                    options.AddPolicy("NotAuthenticated", policy => {
                        policy.RequireAssertion(context => { return !context.User.Identity.IsAuthenticated; });
                    
                    });
                }
            );

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
            });

            #endregion Enable identity


            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
            });

            return services;
        }
    }
}
