using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using MyProgram;

namespace IntegrationTestPlayground;

public class ContainerTests : IAsyncLifetime
{
    private const string NetworkName = "live_live";
    private const string NetworkId = "d0cc92bbd8b072aea60d4cd43e69e179cafc1ae5a4f5aed036ecc5d6d6c4a43e";
    private readonly MsSqlTestcontainer _databaseContainer;

    private readonly TestcontainersContainer _programContainer;

    private int _hostPort;

    private readonly ILogger<DatabaseHacker> _logger;

    public ContainerTests()
    {
        _hostPort = new Random().Next(10000, 60000);
        var loggerFactory = LoggerFactory.Create(b =>
        {
            b.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("SampleApp.Program", LogLevel.Debug)
                .AddConsole();
        });

        _logger = loggerFactory.CreateLogger<DatabaseHacker>();

        _databaseContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
            .WithPortBinding(_hostPort, 1433)
            .WithNetwork(NetworkId, NetworkName)
            .WithHostname("my-program-sqlserver")
            .WithNetworkAliases("my-program-sqlserver-alias")
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Password = "yourStrong(!)Password"
            }).Build();

        _programContainer = new TestcontainersBuilder<TestcontainersContainer>()
            .WithImage("myprogram")
            .WithNetwork(NetworkId, NetworkName)
            .WithHostname("my-program")
            .WithNetworkAliases("my-program-alias")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _databaseContainer.StartAsync();
        await _programContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await _databaseContainer.StopAsync();
        await _programContainer.StopAsync();
    }

    private string HackConnectionString(string connectionString, string containerName)
    {
        return connectionString.Replace("localhost", containerName);
    }

    [Fact]
    public async Task TestContainer()
    {
        var newConnectionString = HackConnectionString(_databaseContainer.ConnectionString, _databaseContainer.Hostname);
        var sut = new DatabaseHacker(_logger, newConnectionString);
        var hackedDatabase = await sut.HackDatabaseSomehow();

        Assert.Contains("Microsoft SQL Server", hackedDatabase);
    }
}