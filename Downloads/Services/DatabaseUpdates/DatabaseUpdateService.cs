namespace Downloads.Services.DatabaseUpdates
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class DatabaseUpdateService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly IDatabaseUpdateTimerService _databaseUpdateTimerService;

        private readonly Func<IServiceProvider, IServiceScope> _serviceScopeProvider;

        public DatabaseUpdateService(IDatabaseUpdateTimerService databaseUpdateTimerService, IServiceProvider serviceProvider)
        {
            _databaseUpdateTimerService = databaseUpdateTimerService;
            _serviceProvider = serviceProvider;
            _serviceScopeProvider = serviceProviderRequiringScope => serviceProviderRequiringScope.CreateScope();
        }

        public DatabaseUpdateService(IDatabaseUpdateTimerService databaseUpdateTimerService, IServiceProvider serviceProvider, Func<IServiceProvider, IServiceScope> serviceScopeProvider)
        {
            _databaseUpdateTimerService = databaseUpdateTimerService;
            _serviceProvider = serviceProvider;
            _serviceScopeProvider = serviceScopeProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await UpdateApps();

            _databaseUpdateTimerService.Start();
            _databaseUpdateTimerService.Elapsed += DatabaseUpdateTimerElapsed;
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            _databaseUpdateTimerService.Stop();
            _databaseUpdateTimerService.Elapsed -= DatabaseUpdateTimerElapsed;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _databaseUpdateTimerService.Dispose();
        }

        private async void DatabaseUpdateTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            await UpdateApps();
        }

        private async Task UpdateApps()
        {
            using (IServiceScope serviceScope = _serviceScopeProvider(_serviceProvider))
            {
                IAppRepositoryUpdateService appRepositoryUpdateService = serviceScope.ServiceProvider.GetService<IAppRepositoryUpdateService>();
                await appRepositoryUpdateService.UpdateApps();
            }
        }
    }
}