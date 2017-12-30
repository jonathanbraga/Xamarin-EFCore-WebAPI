using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VanEscolar.Data;
using Microsoft.EntityFrameworkCore;
using VanEscolar.Domain;
using Microsoft.AspNetCore.Identity;
using VanEscolar.Constants;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace VanEscolar.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(p =>
            p.UseSqlServer(Configuration.GetConnectionString("DBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthorization(o =>
           {
               o.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
           });

            services.AddAuthentication(o =>
           {
               o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
           }).AddJwtBearer( b =>
           {
               b.Audience = "https://jonathanbraga.com.br";
               b.SaveToken = true;
               b.IncludeErrorDetails = true;

               b.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidIssuer = "https://jonathanbraga.com.br",
                   ValidAudience = "https://jonathanbraga.com.br",
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("314159265358979323846264338327")),
                   ValidateLifetime = true

               };
           });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var result = DatabaseMigration.MigrateDatabaseToLatestVersionAsync(app.ApplicationServices).Result;
            
            if (!result.IsSuccess)
            {
                app.Run(async context =>
                {
                    await context.Response.WriteAsync($"Migrations have failed.\nMessage: {result.Message}");
                });
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(configuration =>
            {
                configuration.MapRoute("MainApiRoute", "api/{controller}/{action}");
            });

            app.UseAuthentication();
        }
    }

    public static class DatabaseMigration
    {
        public static async Task<MigrationsResult> MigrateDatabaseToLatestVersionAsync(IServiceProvider serviceProvider)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                // Migrate database to latest version
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var task = db.Database.MigrateAsync();
                var result = await task.ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        return new MigrationsResult(false, t.Exception.Message);
                    }

                    // Seed database
                    if (!db.Roles.Any(p => p.NormalizedName == Roles.Parent.ToUpper()))
                    {
                        db.Roles.Add(new IdentityRole { Name = Roles.Parent, NormalizedName = Roles.Parent.ToUpper() });
                        db.SaveChanges();
                    }

                    return new MigrationsResult(true);
                });
                return result;
            }
        }
    }

    public class MigrationsResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public MigrationsResult(bool isSuccess, string message = "")
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
