

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class AppConfig
{
    private static string _configFilePath = "config.yaml";

    public string NetworkDriveLetter { get; set; }

    public string AzStorageName { get; set; } = "";

    public string AzFileShareName { get; set; } = "";

    public string AkvName { get; set; } = "";

    public string AkvStorageKeySecretName { get; set; } = "";

    public string LawWorkspaceId { get; set; } = "";

    public string AkvLAWKeySecretName { get; set; } = "";

    public string LawTableName { get; set; } = "";

    public bool HideConsoleWindow { get; set; } = false;

    //internal usage
    public string NetworkDriveUNCPath { get; set; } = "";

    //secrets
    public string StorageKey { get; set; } = "";
    public string LAWKey { get; set; } = "";  //optional

    public static AppConfig Create()
    {
        string startupPath = AppDomain.CurrentDomain.BaseDirectory;

        string yamlText = File.ReadAllText(Path.Combine(startupPath, _configFilePath));

        var deserializer = new DeserializerBuilder()
        .WithNamingConvention(new CamelCaseNamingConvention())  // camel casing by default
        .Build();

        var appconfig = deserializer.Deserialize<AppConfig>(yamlText);

        appconfig.NetworkDriveUNCPath = @$"\\{appconfig.AzStorageName}.file.core.windows.net\{appconfig.AzFileShareName}";
        

        return appconfig;
    }

    public void InitSecrets(AzKeyVault akv)
    {
        StorageKey = akv.GetSecret(AkvStorageKeySecretName);

        if(!string.IsNullOrEmpty(AkvLAWKeySecretName))
            LAWKey = akv.GetSecret(AkvLAWKeySecretName);
    }
}