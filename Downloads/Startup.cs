namespace Downloads
{
    using Downloads.Models.Database;
    using Downloads.Models.Repositories;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Routing;
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
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(_configuration["Data:Identity:ConnectionString"]));

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddTransient<IAppRepository, AppRepository>();

            services.AddMvc();
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
                                template: "Download/{App}",
                                defaults: new
                                {
                                    Controller = "Downloads",
                                    Action = "Download"
                                });

                routes.MapRoute(name: null,
                                template: "{Controller=Downloads}/{Action=All}");
            });
        }
    }
}