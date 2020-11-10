using System;
using Ao3Reader.Interfaces;

namespace Ao3Reader.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        private const string BaseUrl = "http://192.168.0.18:7071/api";
        public string GetConfigKey(string key)
        {
            return key switch
            {
                nameof(BaseUrl) => BaseUrl,
                _ => throw new NotImplementedException("Requested key doesn't exist")
            };
        }
    }
}