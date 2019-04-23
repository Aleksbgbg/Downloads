namespace Downloads
{
    using System;

    using Downloads.Infrastructure.Options;
    using Downloads.Models.Database;
    using Downloads.Models.Repositories;
    using Downloads.Services;
    using Downloads.Services.DatabaseUpdates;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using Octokit;

    using WebMarkupMin.AspNetCore2;

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

            services.AddTransient<IGitHubClient, GitHubClient>(serviceProvider =>
            {
                OctokitOptions octokitOptions = serviceProvider.GetService<IOptions<OctokitOptions>>().Value;

                return new GitHubClient(new ProductHeaderValue(octokitOptions.AppName))
                {
                    Credentials = new Credentials(octokitOptions.Username, octokitOptions.Password)
                };
            });

            services.AddTransient<IAppRepository, AppRepository>();
            services.AddTransient<IGitHubApiService, GitHubApiService>();

            services.AddTransient<ITimer, TimerAdapter>();

            services.AddTransient<ITimeIntervalCalculatorService, TimeIntervalCalculatorService>();
            services.AddTransient<IDatabaseUpdateTimerService, DatabaseUpdateTimerService>();
            services.AddTransient<IAppRepositoryUpdateService, AppRepositoryUpdateService>();

#if !DEBUG
            services.AddHostedService<DatabaseUpdateService>();
#endif

            services.AddMvc();

            services.AddWebMarkupMin(options =>
                    {
                        options.AllowMinificationInDevelopmentEnvironment = true;
                        options.AllowCompressionInDevelopmentEnvironment = true;
                    })
                    .AddHtmlMinification()
                    .AddHttpCompression();

            services.Configure<DatabaseTimerOptions>(options =>
            {
                DateTime today = DateTime.Today;
                options.UpdateTime = new DateTime(today.Year, today.Month, today.Day, 22, 00, 00);
            });
            services.Configure<OctokitOptions>(options =>
            {
                options.AppName = "Downloads";
                options.Username = Environment.GetEnvironmentVariable("Octokit_Username");
                options.Password = Environment.GetEnvironmentVariable("Octokit_Password");
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseWebMarkupMin();

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: null,
                                template: "App/View/{AppName}",
                                defaults: new
                                {
                                    Controller = "App",
                                    Action = "ViewApp"
                                });

                routes.MapRoute(name: null,
                                template: "{Controller=App}/{Action=All}");
            });
        }
    }
}