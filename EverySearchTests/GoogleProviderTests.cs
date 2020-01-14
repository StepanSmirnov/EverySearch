using EverySearch.Lib;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static System.Web.HttpUtility;

namespace EverySearchTests
{
    [TestFixture]
    class GoogleProviderTests
    {
        private readonly string resourcesDir = "EverySearchTests.Resources.";
        private ConfigurationStub configuration;

        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationStub();
            configuration["Google:key"] = "keykeykeykey";
            configuration["Google:cx"] = "cxcxcxcxcxcx";
        }

        [Test]
        public void MakeRequest_CorrectParameters_ReturnsTrue()
        {
            SearchProvider search = new GoogleProvider(configuration);
            string query = "Bible";
            int num = 7;
            string host = "https://www.googleapis.com/customsearch/v1";
            string key = UrlEncode(configuration["Google:key"]);
            string cx = UrlEncode(configuration["Google:cx"]);
            string expected = $"{host}?q={query}&cx={cx}&key={key}&num={num}";
            string url = search.MakeRequest(query, num).Address.AbsoluteUri;
            Console.WriteLine($"got:\n{url}");
            Console.WriteLine($"expected:\n{expected}");
            Assert.IsTrue(url == expected);
        }

        [Test]
        public void ParseResult_CorrectJson_ReturnsTrue()
        {
            SearchProvider search = new GoogleProvider(configuration);
            string filename = "google.json";
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            var reader = new StreamReader(thisAssembly.GetManifestResourceStream(resourcesDir + filename));
            string json = reader.ReadToEnd();
            var response = search.ParseResult(json);
            Assert.IsTrue(response.Any(r => r.Snippet.Contains("Bible", StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}
