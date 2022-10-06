
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

partial class Program {

    static IEventLogger _logger = new EventLogger();

    static void Main(string[] args) {

        _logger.Info("AzFile-Keyless-Mount started, loading config.yaml");

        System.AppDomain.CurrentDomain.UnhandledException+=UnhandledExceptionTrapper;

        AppConfig appconfig = AppConfig.Create();

        if(!IsConfigInputIsValid(appconfig))
        {
            _logger.Error($"AppConfig error: Drive Letter {appconfig.NetworkDriveLetter} or UNC Path is invalid {appconfig.NetworkDriveUNCPath}");
            Environment.Exit(0);
        }

        if(appconfig.HideConsoleWindow)
            ConsoleWindowHider.HideWindow();

        _logger.Info("config.yaml loaded");

        var akv = new AzKeyVault(appconfig.AkvName, _logger);

         _logger.Info("initializing secrets");

        appconfig.InitSecrets(akv);

        _logger.Info("initialize secrets completed");

        //init logger to also send logs to Log Analytics
        _logger.SetLogToLAW(appconfig.LawWorkspaceId, appconfig.LAWKey, appconfig.LawTableName);

        var cmdRunner = new CmdRunner(appconfig, _logger);

        _logger.Info("starting worker to monitor network drive");

        IHost host =  Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddHostedService(services => {
                    return new BackgroundWorker(appconfig, cmdRunner, _logger);
                });
            })
        .Build();

        host.Run();
    }

    static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e) {
        _logger.Error(e.ExceptionObject.ToString());
        Environment.Exit(0);
    }

        // prevent command injection
    private static bool IsConfigInputIsValid(AppConfig appconfig)
    {
        char c;
        bool OK = Char.TryParse(appconfig.NetworkDriveLetter, out c);

        if(OK && IsUncPath(appconfig.NetworkDriveUNCPath))
            return true;
        return false;
    }

    public static bool IsUncPath(string path)
    {
        bool OK = Uri.TryCreate(path, UriKind.Absolute, out Uri uri) && uri.IsUnc;
        return OK;
    }
}






