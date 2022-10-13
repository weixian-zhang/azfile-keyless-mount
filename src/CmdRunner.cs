
using System.Diagnostics;

public class CmdRunner
{
    private AppConfig _appconfig;
    private IEventLogger _logger;

    public CmdRunner(AppConfig appconfig, IEventLogger logger)
    {
        _appconfig = appconfig;
        _logger = logger;
    }

    public void MapAzFileNetworkDrive()
    {
        _logger.Info($"mapping network drive");

        string cmd = $"/C net use {_appconfig.NetworkDriveLetter}: {_appconfig.NetworkDriveUNCPath} {_appconfig.StorageKey} /user:localhost\\{_appconfig.AzStorageName}";

        var process = CreateProcess(cmd);

        bool pStarted = process.Start();

        if(pStarted)
            _logger.Info($"network drive map process started successfully");
        else
            _logger.Info($"network drive map process failed to start");
    }

    public void UnmapAzFileNetworkDrive()
    {
        _logger.Info($"unmapping network drive");

        if(!IsNetworkDriveExist())
        {
            _logger.Info($"cannot unmap, network drive does not exist: {_appconfig.NetworkDriveUNCPath}");
            return;
        }

        string cmd = $"/C net use {_appconfig.NetworkDriveLetter}: /delete /y";

        var process = CreateProcess(cmd);

        bool pStarted = process.Start();

        if(pStarted)
            _logger.Info($"network drive unmap process started successfully");
        else
            _logger.Info($"network drive unmap process failed to start");
    }

    public bool IsNetworkDriveExist()
    {
        if(Directory.Exists($"{_appconfig.NetworkDriveLetter}:"))
            return true;
        
        return false;
    }

    private Process CreateProcess(string cmd)
    {
        var process = new Process();
        
        var startInfo = new ProcessStartInfo();
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.FileName = "cmd.exe";
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;
        startInfo.Arguments = cmd;

        process.OutputDataReceived +=   DataReceivedEventHandler;
        process.ErrorDataReceived += ErrorReceivedEventHandler;

        process.StartInfo = startInfo;

        return process;
    }

    private void DataReceivedEventHandler(object sender, DataReceivedEventArgs e)
    {
        _logger.Info(e.Data);
    }

    private void ErrorReceivedEventHandler(object sender, DataReceivedEventArgs e)
    {
        _logger.Error(e.Data);
    }
}