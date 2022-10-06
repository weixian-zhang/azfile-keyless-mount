using Serilog;
using Serilog.Core;

public interface IEventLogger
{
    void Info(string message);
    void Error(Exception ex);
    void Error(string error);

    void SetLogToLAW(string workspaceId, string workspaceKey, string LawTableName);
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

    public void SetLogToLAW(string workspaceId, string workspaceKey, string LawTableName)
    {
        if(!ShouldLogToLAW(workspaceId, workspaceKey, LawTableName))
            return;

        var loggerConfigWithoutLaw = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.AzureAnalytics(workspaceId, workspaceKey, LawTableName)
            .WriteTo.File(_logFilePath);

        _logger = loggerConfigWithoutLaw.CreateLogger();
    }

    private bool ShouldLogToLAW(string workspaceId, string workspaceKey, string LawTableName)
    {
        if (!string.IsNullOrEmpty(workspaceId) && 
            !string.IsNullOrEmpty(workspaceKey) && 
            !string.IsNullOrEmpty(LawTableName))
            return true;
        
        return false;
    }
}