using EverySearch.Lib;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Web.HttpUtility;

namespace EverySearchTests
{
    class YandexProviderTests
    {
        private readonly string resourcesDir = "EverySearchTests.Resources.";
        private ConfigurationStub configuration;

        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationStub();
            configuration["Yandex:key"] = "keykeykeykey";
            configuration["Yandex:user"] = "useruseruser";
        }

        [Test]
        public void MakeRequest_CorrectParameters_ReturnsTrue()
        {
            SearchProvider search = new YandexProvider(configuration);
            string query = "Bible";
            int num = 7;
            string host = "https://yandex.com/search/xml";
            string key = UrlEncode(configuration["Yandex:key"]);
            string user = UrlEncode(configuration["Yandex:user"]);
            string expected = $"{host}?query={query}&key={key}&user={user}&groupby=attr%3D%22%22.mode%3Dflat.groups-on-page%3D{num}.docs-in-group%3D1";
            string url = search.MakeRequest(query, num).Address.AbsoluteUri;
            Console.WriteLine($"got:\n{url}");
            Console.WriteLine($"expected:\n{expected}");
            Assert.IsTrue(url == expected);
        }

        [Test]
        public void ParseResult_CorrectJson_ReturnsTrue()
        {
            SearchProvider search = new YandexProvider(configuration);
            string filename = "yandex.xml";
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            var reader = new StreamReader(thisAssembly.GetManifestResourceStream(resourcesDir + filename));
            string xml = reader.ReadToEnd();
            var response = search.ParseResult(xml);
            Assert.IsTrue(response.Any(r => r.Snippet.Contains("Bible", System.StringComparison.InvariantCultureIgnoreCase)));
        }

        [Test]
        public void ParseResult_EmptyJson_Throws()
        {
            SearchProvider search = new YandexProvider(configuration);
            string json = string.Empty;
            Assert.Throws<ArgumentException>(() => search.ParseResult(json));

        }

        [Test]
        public void ParseResult_ErrorJson_Throws()
        {
            SearchProvider search = new YandexProvider(configuration);
            string json = "<?xml version=\"1.0\" encoding=\"utf-8\"?><yandexsearch version=\"1.0\"><response date=\"20200114T135529\"><error code=\"33\">message</error><reqid>1579</reqid></response></yandexsearch>";
            Assert.Throws<ArgumentException>(() => search.ParseResult(json));
        }
    }
}
