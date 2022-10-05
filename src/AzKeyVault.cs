
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

public class AzKeyVault
{
    private string _akvName;
    private SecretClient _akvSecretCLient;
    private IEventLogger _logger;

    public AzKeyVault(string akvName, IEventLogger logger)
    {
        _akvName = akvName;
        _logger = logger;

        try
        {
            string akvUrl = $"https://{_akvName}.vault.azure.net/";

            _logger.Info($"Initializing connection to Key Vault {akvUrl}");

            var cred = new DefaultAzureCredential(new DefaultAzureCredentialOptions());

            _akvSecretCLient = new SecretClient(
                new Uri(akvUrl), 
                cred
            );

            _logger.Info($"Connecting to Key Vault {akvUrl} using managed Identity");
        }
        catch(Exception ex)
        {
            _logger.Error(ex);
            throw;
        }
    }

    public string GetSecret(string secretName)
    {
        try
        {
            _logger.Info($"getting secret by name {secretName}");

            var response = _akvSecretCLient.GetSecret(secretName);

            string secret = response.Value.Value;

            _logger.Info($"secret {secretName} retrieved successfully");

            return secret;
        }
        catch (Exception ex)
        {
            _logger.Error(ex);
            throw;
        }
    }
}
