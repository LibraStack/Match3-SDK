using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Match3.Template.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Terminal.Match3.Extensions;
using Terminal.Match3.GameModes;
using Terminal.Match3.Services;

namespace Terminal.Match3
{
    [SupportedOSPlatform("linux")]
    [SupportedOSPlatform("windows")]
    internal static class Program
    {
        public static async Task Main()
        {
            await Host
                .CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<GameService>();
                    services.AddTransient<IGameMode, GameInitMode>();
                    services.AddTransient<IGameMode, GamePlayMode>();
                    services.AddInfrastructure();
                })
                .RunConsoleAsync(options =>
                {
                    options.SetCursorVisible(false);
                    options.SuppressStatusMessages();
                    options.SetOutputEncoding(Encoding.UTF8);
                });
        }
    }
}