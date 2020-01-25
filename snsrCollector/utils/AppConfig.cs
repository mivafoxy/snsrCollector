using Microsoft.Extensions.Configuration;
using System;

namespace snsrCollector.utils
{
    /// <summary>
    /// Инкапсуляция логики по работе с файлом конфигурации.
    /// Файл конфигурации статичен, данный класс является хранилищем его пропертей. 
    /// При изменении файла конфигурации соответственно перезапускается всё приложение.
    /// </summary>
    public class AppConfig
    {

        private readonly IConfiguration configuration;

        public AppConfig(string configFilePath)
        {
            // Путь к файлу конфигурации. По умолчанию файл конфигурации должен лежать рядом с проектом...
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddIniFile(configFilePath, false, true);
            configuration = configBuilder.Build();
        }

        public bool IsKeyEnabled(string key)
        {
            bool hasKey = configuration.GetSection(key).Exists();

            if (!hasKey)
                return false;

            bool isEnabled = bool.TryParse(GetKeyValueOr(key, "false"), out bool succed);

            if (!succed)
                return false;

            return isEnabled;
        }

        public int GetKeyValueOrAsInt(string key, int returnValue)
        {
            int value =
                int.Parse(
                    GetKeyValueOr(
                        key,
                        returnValue.ToString()));

            return value;
        }

        public string GetKeyValueOr(string key, string returnValue)
        {
            string keyValue = configuration.GetSection(key).Value;

            if (keyValue is null)
                return returnValue;
            else
                return keyValue;
        }
    }
}
