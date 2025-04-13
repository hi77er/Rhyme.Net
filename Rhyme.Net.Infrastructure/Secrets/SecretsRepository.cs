using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace Rhyme.Net.Infrastructure.Secrets;

public class SecretsRepository
{
    private readonly IAmazonSecretsManager _secretsManager;

    public SecretsRepository(RegionEndpoint region)
    {
        _secretsManager = new AmazonSecretsManagerClient(region);
    }

    /// <summary>
    /// Retrieves the secret value from AWS Secrets Manager.
    /// </summary>
    /// <param name="secretName">The name of the secret to retrieve.</param>
    /// <returns>The secret value as a string.</returns>
    public async Task<string> GetSecretAsync(string secretName)
    {
        try
        {
            var request = new GetSecretValueRequest()
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT", // VersionStage defaults to AWSCURRENT if unspecified.
            };

            var response = await _secretsManager.GetSecretValueAsync(request);

            return response.SecretString ?? throw new Exception("Secret value is null.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to retrieve secret: {ex.Message}");
        }
    }
}