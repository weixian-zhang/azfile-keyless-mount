
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

partial class Program {

    static IEventLogger _logger = new EventLogger();

    static void Main(string[] args) {

        _logger.Info("AzFile-Keyless-Mount started, loading config.yaml");

        System.AppDomain.CurrentDomain.UnhandledException+=UnhandledExceptionTrapper;

        AppConfig appconfig = AppConfig.Create();

        _logger.Info("config.yaml loaded");

        var akv = new AzKeyVault(appconfig.AkvName, _logger);

         _logger.Info("initializing secrets");

        appconfig.InitSecrets(akv);

        _logger.Info("initialize secrets completed");

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
}

// See https://aka.ms/new-console-template for more information






