using Serilog;
using Serilog.Core;

public interface IEventLogger
{
    void Info(string message);
    void Error(Exception ex);
    void Error(string error);
}

public class EventLogger : IEventLogger
{
    private AzKeyVault _akv;

    private string _logFilePath = "logs/log.txt";

    private Logger _logger;
    
    public EventLogger()
    {
        var loggerConfigWithoutLaw = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File(_logFilePath);

        _logger = loggerConfigWithoutLaw.CreateLogger();

        //_akv = akv;

        // if(ShouldLogToLAW())
        // {
        //     string lawKey = _akv.GetSecret(_appconfig.AkvLAWKeySecretUri);

        //      = new LoggerConfiguration()
        //     .WriteTo.Console()
        //     .WriteTo.File(_logFilePath)
        //     .WriteTo.AzureLogAnalytics(_appconfig.LogAnalyticsWorkspaceId, lawKey)
        //     .CreateLogger();
        // }
        // else        
    }

    public void Error(Exception ex)
    {
       _logger.Error(ex.StackTrace);
    }

    public void Error(string error)
    {
       _logger.Error(error);
    }

    public void Info(string message)
    {
        _logger.Information(message);
    }

    public void SetLogToLAW(string workspaceId, string workspaceKey)
    {
        var loggerConfigWithoutLaw = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.AzureAnalytics(workspaceId, workspaceKey)
            .WriteTo.File(_logFilePath);

        _logger = loggerConfigWithoutLaw.CreateLogger();
    }

    private bool ShouldLogToLAW(string workspaceId, string workspaceKey)
    {
        if (!string.IsNullOrEmpty(workspaceId) && !string.IsNullOrEmpty(workspaceKey))
            return true;
        
        return false;
    }
}