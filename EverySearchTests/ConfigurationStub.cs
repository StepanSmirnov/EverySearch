using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace EverySearchTests
{
    public class ConfigurationStub : IConfiguration
    {
        private Dictionary<string, string> valuePairs;
        public ConfigurationStub()
        {
            valuePairs = new Dictionary<string, string>();
        }

        public string this[string key] { get => valuePairs[key]; set => valuePairs[key] = value; }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }
}
