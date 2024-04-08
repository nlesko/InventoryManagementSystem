using System.Reflection;
using System.Text;

using Carter;

using FluentValidation;

using InventoryManagementSystemApi.API.Common.Behaviours;
using InventoryManagementSystemApi.API.Common.Interfaces;
using InventoryManagementSystemApi.API.Domain.Entities;
using InventoryManagementSystemApi.API.Infrastructure.Persistence;
using InventoryManagementSystemApi.API.Infrastructure.Persistence.Interceptors;
using InventoryManagementSystemApi.API.Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace InventoryManagementSystemApi.API;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddCarter();
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            options.AddOpenBehavior(typeof(AuthorizationBehaviour<,>));
            options.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            options.AddOpenBehavior(typeof(PerformanceBehaviour<,>));
            options.AddOpenBehavior(typeof(UnhandledExceptionBehaviour<,>));
        });
        
        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        services.AddDbContext<ApplicationDbContext>(options =>
                 {
                     var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                     string connStr;

                     // Depending on if in development or production, use either Heroku-provided
                     // connection string, or development connection string from env var.
                     if (env == "Development")
                     {
                         // Use connection string from file.
                         connStr = configuration.GetConnectionString("DefaultConnection");
                     }
                     else
                     {
                         // Use connection string provided at runtime by Heroku.
                         var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

                         // Parse connection URL to connection string for Npgsql
                         connUrl = connUrl?.Replace("postgres://", string.Empty);
                         var pgUserPass = connUrl?.Split("@")[0];
                         var pgHostPortDb = connUrl?.Split("@")[1];
                         var pgHostPort = pgHostPortDb?.Split("/")[0];
                         var pgDb = pgHostPortDb?.Split("/")[1];
                         var pgUser = pgUserPass?.Split(":")[0];
                         var pgPass = pgUserPass?.Split(":")[1];
                         var pgHost = pgHostPort?.Split(":")[0];
                         var pgPort = pgHostPort?.Split(":")[1];

                         connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb}; SSL Mode=Require; Trust Server Certificate=true";
                     }
                 });
                 
        services.AddIdentityCore<ApplicationUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            opt.SignIn.RequireConfirmedEmail = true;
        })
        .AddRoles<IdentityRole<int>>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager<SignInManager<ApplicationUser>>()
        .AddDefaultTokenProviders();;
        var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"]));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chat")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        // services.AddScoped<IDomainEventService, DomainEventService>();

        services.AddTransient<IDateTimeService, DateTimeService>();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}