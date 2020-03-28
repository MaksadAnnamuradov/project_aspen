using System;
using System.Data;
using Aspen.Core.Data;
using Aspen.Core.Repositories;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aspen.Core.Services;
using Npgsql;
using System.Threading;

namespace Aspen.Api
{
    public class Startup
    {
        private System.Func<NpgsqlConnection> getDbConnection;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add Postgres support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(connectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(FirstMigration).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            getDbConnection = () => new NpgsqlConnection(connectionString);
            services.AddTransient<Func<IDbConnection>>(c => getDbConnection);

            services.AddScoped<ICharityRepository, CharityRepository>();
            services.AddScoped<IThemeRepository, ThemeRepository>();
            services.AddControllers().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMigrationRunner migrationRunner)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            migrationRunner.MigrateUp();

            //SeedService.SeedAll(new CharityRepository(getDbConnection));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // endpoints.MapControllerRoute(
                //     "Default",
                //     "{controller}/{action}/{id}",
                //     new { controller = "Home", action = "Get", id = ""},
                //     new { TenantAccess = new CharityRouteConstraint(new CharityRepository(getDbConnection)) } );
                // endpoints.MapControllerRoute(
                //     "Global Admin",
                //     "/admin/{controller}/{action}",
                //     new { controller = "Chartiy", action = "Get"},
                //     new { TenantAccess = new AdminRouteConstraint() } );
            });
        }
    }
}
