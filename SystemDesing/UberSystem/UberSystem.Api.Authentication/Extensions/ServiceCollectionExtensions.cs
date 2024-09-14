using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UberSystem.Domain.Entities;
using UberSystem.Domain.Interfaces;
using UberSystem.Domain.Interfaces.Services;
using UberSystem.Domain.Models;
using UberSystem.Infrastructure;
using UberSystem.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace UberSystem.Api.Authentication.Extensions
{
   	public static class ServiceCollectionExtensions
	{
    	/// <summary>
    	/// Add needed instances for database
    	/// </summary>
    	/// <param name="services"></param>
    	/// <param name="configuration"></param>
    	/// <returns></returns>
    	public static IServiceCollection AddDatabase(this IServiceCollection services,ConfigurationManager  configuration)
    	{
        	// Configure DbContext with Scoped lifetime  
            services.AddDbContext<UberSystemDbContext>(options =>
            	{
                    options.UseSqlServer(configuration.GetConnectionString("Default"),
                    	sqlOptions => sqlOptions.CommandTimeout(120));
                    // options.UseLazyLoadingProxies();
            	}
        	);
 
        	services.AddScoped<Func<UberSystemDbContext>>((provider) => () => provider.GetService<UberSystemDbContext>());
            services.AddScoped<DbFactory>();
        	services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserService, UserService>();
    //        services.AddIdentity<User, IdentityRole<int>>()
				//.AddEntityFrameworkStores<UberSystemDbContext>()
				//.AddDefaultTokenProviders();
			// Configure JWT authentication
			var jwtSettings = configuration.GetSection("JwtSettings");
			var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
        	return services;
    	}
 
    	/// <summary>
    	/// Add instances of in-use services
    	/// </summary>
    	/// <param name="services"></param>
    	/// <returns></returns>
    	//public static IServiceCollection AddServices(this IServiceCollection services)
    	//{
     //   	return services.AddScoped<ICabService, CabService>();
    	//}
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICabService, CabService>();
            services.AddScoped(typeof(TokenService));

            return services;
        }
    }
}