using EverySearch.Lib;
using EverySearch.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace EverySearchTests
{
    public class ProvidersTest
    {
        private readonly string resourcesDir = "EverySearchTests.Resources.";
        private ConfigurationStub configuration;

        [SetUp]
        public void Setup()
        {
            configuration = new ConfigurationStub();
            configuration["Google:key"] = "keykeykeykey";
            configuration["Google:cx"] = "cxcxcxcxcxcx";
            configuration["Yandex:key"] = "keykeykeykey";
            configuration["Yandex:user"] = "useruseruser";
            configuration["Bing:key"] = "keykeykeykey";
        }

        [Test]
        public void TestGoogleUrl()
        {
            SearchProvider search = new GoogleProvider(configuration);
            string query = "Bible";
            int num = 7;
            string host = "https://www.googleapis.com/customsearch/v1";
            string key = System.Web.HttpUtility.UrlPathEncode(configuration["Google:key"]);
            string cx = System.Web.HttpUtility.UrlPathEncode(configuration["Google:cx"]);
            string expected = $"{host}?q={query}&cx={cx}&key={key}&num={num}";
            string url = search.MakeRequest(query, num).Address.AbsoluteUri;
            Console.WriteLine($"got:\n{url}");
            Console.WriteLine($"expected:\n{expected}");
            Assert.IsTrue(url == expected);
        }

        [Test]
        public void TestGoogleParse()
        {
            SearchProvider search = new GoogleProvider(configuration);
            string filename = "google.json";
            string json = ReadResource(resourcesDir + filename);
            var response = search.ParseResult(json);
            Assert.IsTrue(response.Any(r => r.Snippet.Contains("Bible", System.StringComparison.InvariantCultureIgnoreCase)));
        }

        [Test]
        public void TestYandexUrl()
        {
            SearchProvider search = new YandexProvider(configuration);
            string query = "Bible";
            int num = 7;
            string host = "https://yandex.com/search/xml";
            string key = System.Web.HttpUtility.UrlPathEncode(configuration["Yandex:key"]);
            string user = System.Web.HttpUtility.UrlPathEncode(configuration["Yandex:user"]);
            string expected=$"{host}?query={query}&key={key}&user={user}&groupby=attr%3D%22%22.mode%3Dflat.groups-on-page%3D{num}.docs-in-group%3D1";
            string url = search.MakeRequest(query, num).Address.AbsoluteUri;
            Console.WriteLine($"got:\n{url}");
            Console.WriteLine($"expected:\n{expected}");
            Assert.IsTrue(url == expected);
        }

        [Test]
        public void TestYandexParse()
        {
            SearchProvider search = new YandexProvider(configuration);
            string filename = "yandex.xml";
            string xml = ReadResource(resourcesDir + filename);
            var response = search.ParseResult(xml);
            Assert.IsTrue(response.Any(r => r.Snippet.Contains("Bible", System.StringComparison.InvariantCultureIgnoreCase)));
        }

        [Test]
        public void TestBingUrl()
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
        public void TestBingParse()
        {
            SearchProvider search = new BingProvider(configuration);
            string filename = "bing.json";
            string json = ReadResource(resourcesDir + filename);
            var response = search.ParseResult(json);
            Assert.IsTrue(response.Any(r => r.Snippet.Contains("Bible", System.StringComparison.InvariantCultureIgnoreCase)));
        }

        private static string ReadResource(string path)
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            Console.WriteLine(String.Join(" ", (thisAssembly.GetManifestResourceNames())));
            var reader = new StreamReader(thisAssembly.GetManifestResourceStream(path));
            string xml = reader.ReadToEnd();
            return xml;
        }
    }
}