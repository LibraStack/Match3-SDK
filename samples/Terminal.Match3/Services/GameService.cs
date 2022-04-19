using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Match3.Template.Extensions;
using Match3.Template.Interfaces;
using Microsoft.Extensions.Hosting;
using Terminal.Match3.Extensions;

namespace Terminal.Match3.Services
{
    public class GameService : IHostedService
    {
        private readonly IEnumerable<IGameMode> _gameModes;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public GameService(IEnumerable<IGameMode> gameModes, IHostApplicationLifetime applicationLifetime)
        {
            _gameModes = gameModes;
            _applicationLifetime = applicationLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(async () =>
            {
                foreach (var gameMode in _gameModes)
                {
                    await gameMode;
                }

                _applicationLifetime.StopApplication();
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var gameMode in _gameModes)
            {
                gameMode.Dispose();
            }

            return Task.CompletedTask;
        }
    }
}