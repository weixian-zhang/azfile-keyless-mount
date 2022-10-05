using Microsoft.Extensions.Hosting;

public class BackgroundWorker : BackgroundService
{
    CmdRunner _cmdRunner;
    IEventLogger _logger;
    AppConfig _appconfig;

    public BackgroundWorker(AppConfig appconfig, CmdRunner cmdRunner, IEventLogger logger)
    {
        _appconfig = appconfig;
        _cmdRunner = cmdRunner;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if(!_cmdRunner.IsNetworkDriveExist())
            {
                _logger.Info($"network drive does not exist: {_appconfig.NetworkDriveUNCPath}");

                _cmdRunner.MapAzFileNetworkDrive();

                _logger.Info($"network drive mapped successfully: {_appconfig.NetworkDriveUNCPath}");
            }

            await Task.Delay(3000, stoppingToken);
        }
    }
}
