namespace Downloads
{
    using System;

    using Downloads.Infrastructure.Octokit;
    using Downloads.Models.Database;
    using Downloads.Models.Repositories;
    using Downloads.Services;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(_configuration["Data:ConnectionString"]));

            services.AddTransient<IAppRepository, AppRepository>();
            services.AddTransient<IGitHubApiService, GitHubApiService>();

            services.AddMvc();

            services.Configure<OctokitOptions>(options =>
            {
                options.AppName = "Downloads";
                options.ClientId = Environment.GetEnvironmentVariable("Octokit_Client_Id");
                options.ClientSecret = Environment.GetEnvironmentVariable("Octokit_Client_Secret");
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: null,
                                template: "Download/{AppName}",
                                defaults: new
                                {
                                    Controller = "Downloads",
                                    Action = "Download"
                                });

                routes.MapRoute(name: null,
                                template: "View/App/{AppName}",
                                defaults: new
                                {
                                    Controller = "Downloads",
                                    Action = "ViewApp"
                                });

                routes.MapRoute(name: null,
                                template: "{Controller=Downloads}/{Action=All}");
            });
        }
    }
}