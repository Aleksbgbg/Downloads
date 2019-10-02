namespace Downloads
{
    using System;

    using Downloads.Infrastructure;
    using Downloads.Infrastructure.Options;
    using Downloads.Models.Database;
    using Downloads.Models.Repositories;
    using Downloads.Services;
    using Downloads.Services.DatabaseUpdates;
    using Downloads.Services.GitHub;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ApplicationModels;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    using Octokit;

    using Swashbuckle.AspNetCore.Swagger;

    using WebMarkupMin.AspNetCore2;

    public class Startup
    {
        private const string SpaStaticFilesPath = "Client/dist/Client";

        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(_configuration["Data:ConnectionString"]));

            services.AddSpaStaticFiles(configuration => configuration.RootPath = SpaStaticFilesPath);

            services.AddTransient<IGitHubClient, GitHubClient>(serviceProvider =>
            {
                OctokitOptions octokitOptions = serviceProvider.GetService<IOptions<OctokitOptions>>().Value;

                return new GitHubClient(new ProductHeaderValue(octokitOptions.AppName))
                {
                    Credentials = new Credentials(octokitOptions.Username, octokitOptions.Password)
                };
            });

            services.AddTransient<IAppRepository, AppRepository>();

            services.AddTransient<IRepositoryFinderService, RepositoryFinderService>();
            services.AddTransient<IReleaseFinderService, ReleaseFinderService>();
            services.AddTransient<IRepositoryToAppGeneratorService, RepositoryToAppGeneratorService>();
            services.AddTransient<IGitHubApiService, GitHubApiService>();

            services.AddTransient<ITimer, TimerAdapter>();

            services.AddTransient<ITimeIntervalCalculatorService, TimeIntervalCalculatorService>();
            services.AddTransient<IDatabaseUpdateTimerService, DatabaseUpdateTimerService>();
            services.AddTransient<IAppRepositoryUpdateService, AppRepositoryUpdateService>();

#if !DEBUG
            // services.AddHostedService<DatabaseUpdateService>();
#endif

            services.AddMvc(options => options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer())))
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddWebMarkupMin(options =>
                    {
                        options.AllowMinificationInDevelopmentEnvironment = true;
                        options.AllowCompressionInDevelopmentEnvironment = true;
                    })
                    .AddHtmlMinification()
                    .AddHttpCompression();

            services.AddSwaggerGen(options => options.SwaggerDoc("v1",
                                                                 new Info
                                                                 {
                                                                     Title = "Downloads API",
                                                                     Version = "v1"
                                                                 }));

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

                app.UseSwagger();
                app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Downloads API V1"));
            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseWebMarkupMin();

            app.UseMvc();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = SpaStaticFilesPath;

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}