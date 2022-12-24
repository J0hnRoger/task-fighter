using Microsoft.Extensions.Configuration;

namespace TaskFighter.Tests;

/// <summary>
/// Méthodes d'accession aux configurations du projet testé
/// soit dans les secrets, soit dans un appSettings.json
/// </summary>
public class TestConfiguration
{
    public static IConfigurationRoot GetIConfigurationRoot()
    {
        var config = new ConfigurationBuilder();
        return config
            .AddUserSecrets<TestConfiguration>()
            .Build();
    }

    /// <summary>
    /// Retourne l'instance passée en paramètre bindée sur la section de la configuration
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="configClass"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static T GetApplicationConfiguration<T>(string sectionName) where T : new()
    {
        var iConfig = GetIConfigurationRoot();
        var configuration = new T();

        iConfig
            .GetSection(sectionName)
            .Bind(configuration);

        return configuration;
    }

    public static string GetSecretValue(string sectionName)
    {
        var iConfig = GetIConfigurationRoot();
        return iConfig
            .GetSection(sectionName)
            .Value;
    }
}