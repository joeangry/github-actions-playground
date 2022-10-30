using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyProgram;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await Host.CreateDefaultBuilder(args)
            .ConfigureLogging(c => c.AddConsole())
            .ConfigureServices(c =>
            {
                c.AddSingleton<DatabaseHacker>(x =>
                {
                    var loggerFactory = new LoggerFactory();
                    var logger = loggerFactory.CreateLogger<DatabaseHacker>();

                    return new DatabaseHacker(logger, args[0]);
                });
                c.AddHostedService<ConsoleHostedService>();
            })
            .RunConsoleAsync();
    }

}