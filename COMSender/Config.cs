using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace COMSender;

public class Config
{
    public static readonly string configFile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.comsender\\config.yaml";
    private static ConfigFileStructure data;
    private static bool hasChanged = false;
    public static readonly string defaultConfig = @"
meta: false
";

    public static void Initialize()
    {
        if (!File.Exists(configFile))
        {
            File.WriteAllText(configFile, defaultConfig);
        }
        
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        string input = File.ReadAllText(configFile);

        data = deserializer.Deserialize<ConfigFileStructure>(input);
    }

    public static void SaveData()
    {
        if (hasChanged)
        {
            var serializer = new SerializerBuilder().Build();
            var yaml = serializer.Serialize(data);
            File.WriteAllText(configFile, yaml);
            hasChanged = false;
        }
    }

    public static bool IsMetaPressed()
    {
        return data.meta;
    }

    public static void SetMeta(bool value)
    {
        hasChanged = true;
        data.meta = value;
    }
}

public class ConfigFileStructure
{
    public bool meta;
}