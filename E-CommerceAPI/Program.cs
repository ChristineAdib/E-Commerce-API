
using Core.Entities.Identity;
using Core.Repositories;
using E_CommerceAPI.Errors;
using E_CommerceAPI.Extensions;
using E_CommerceAPI.Helpers;
using E_CommerceAPI.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Data;
using Repository.Identity;
using StackExchange.Redis;

namespace E_CommerceAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<EContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");
                return ConnectionMultiplexer.Connect(Connection);
            });

            builder.Services.AddApplicationServices();

            builder.Services.AddIdentityServices(builder.Configuration); 
            #endregion

            var app = builder.Build();

            #region Update database

            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;

            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                var dbContext = Services.GetRequiredService<EContext>();
                await dbContext.Database.MigrateAsync();

                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync();

                #region Data Seeding
                var roleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();
                await AppIdentityDbContextSeed.SeedRolesAsync(roleManager);
                var UserManager = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);

                await EContextSeed.SeedAsync(dbContext);
   
                #endregion
            }
            catch (Exception ex) 
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An Error Occurred During Update Database");
            }

            #endregion


            #region Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>();
                app.MapOpenApi();
            }
            app.UseStatusCodePagesWithRedirects("/error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();


            #endregion    
            
            app.Run(); 
        }
    }
}
