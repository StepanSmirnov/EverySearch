using EverySearch.Lib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EverySearchTests
{
    [TestFixture]
    class BingProviderTests
    {
        private readonly string resourcesDir = "EverySearchTests.Resources.";
        private ConfigurationStub configuration;

        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationStub();
            configuration["Bing:key"] = "keykeykeykey";
        }

        [Test]
        public void MakeRequest_CorrectParameters_ReturnsTrue()
        {
            SearchProvider search = new BingProvider(configuration);
            string query = "Bible";
            int count = 7;
            string host = "https://api.cognitive.microsoft.com/bing/v5.0/search";
            string expected = $"{host}?q={query}&count={count}";
            var request = search.MakeRequest(query, count);
            string url = request.Address.AbsoluteUri;
            Console.WriteLine($"got:\n{url}");
            Console.WriteLine($"expected:\n{expected}");
            Assert.IsTrue(url == expected && request.Headers["Ocp-Apim-Subscription-Key"] == configuration["Bing:key"]);
        }

        [Test]
        public void ParseResult_CorrectJson_ReturnsTrue()
        {
            SearchProvider search = new BingProvider(configuration);
            string filename = "bing.json";
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            var reader = new StreamReader(thisAssembly.GetManifestResourceStream(resourcesDir + filename));
            string json = reader.ReadToEnd();
            var response = search.ParseResponse(json);
            Assert.IsTrue(response.Any(r => r.Snippet.Contains("Bible", StringComparison.InvariantCultureIgnoreCase)));
        }

        [Test]
        public void ParseResult_EmptyJson_Throws()
        {
            SearchProvider search = new BingProvider(configuration);
            string json = string.Empty;
            Assert.Throws<ArgumentException>(() => search.ParseResponse(json));

        }

        [Test]
        public void ParseResult_ErrorJson_Throws()
        {
            SearchProvider search = new BingProvider(configuration);
            string json = "{\"error\":{\"code\":\"401\",\"message\":\"message\"}}";
            Assert.Throws<ArgumentException>(() => search.ParseResponse(json));
        }
    }
}
