using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyProgram;

public class ConsoleHostedService : IHostedService
{
    private readonly ILogger<ConsoleHostedService> _logger;
    private readonly DatabaseHacker _databaseHacker;

    public ConsoleHostedService(ILogger<ConsoleHostedService> logger, DatabaseHacker databaseHacker)
    {
        _logger = logger;
        _databaseHacker = databaseHacker;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        do
        {
            var retVal = await _databaseHacker.HackDatabaseSomehow();
            _logger.LogInformation("Got result from query: {Query}", retVal);

            await Task.Delay(3000, cancellationToken);
        } while (cancellationToken.IsCancellationRequested);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}