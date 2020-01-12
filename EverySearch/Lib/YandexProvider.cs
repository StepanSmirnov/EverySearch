using EverySearch.Models;
using EverySearch.Lib;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

namespace EverySearch.Lib
{
    public class YandexProvider : SearchProvider
    {
        private string key;
        private string user;
        private readonly string host = "https://yandex.com/search/xml";
        private readonly byte maxResults = 100;

        public YandexProvider(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void InitializeCredentials(IConfiguration configuration)
        {
            key = System.Web.HttpUtility.UrlPathEncode(configuration["Yandex:key"]);
            user = System.Web.HttpUtility.UrlPathEncode(configuration["Yandex:user"]);
        }

        public override HttpWebRequest MakeRequest(string query, int? count)
        {
            string html = string.Empty;
            int num = count != null ? Math.Clamp(count.Value, 1, maxResults) : maxResults;
            string encodedQuery = System.Web.HttpUtility.UrlPathEncode(query);
            string url = $"{host}?query={encodedQuery}&key={key}&user={user}&groupby=attr%3D%22%22.mode%3Dflat.groups-on-page%3D{num}.docs-in-group%3D1";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            return request;
        }

        public override IEnumerable<SearchResult> ParseResult(string result)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            XmlElement root = doc.DocumentElement;
            var error = root.SelectSingleNode("/yandexsearch/response/error");
            if (error != null)
            {
                throw new ArgumentException(error.InnerText);
            }
            var docs = root.SelectNodes("/yandexsearch/response/results/grouping//doc");
            List<SearchResult> searchResults = new List<SearchResult>();
            for (int i = 0; i < docs.Count; i++)
            {
                SearchResult singleResult = new SearchResult();
                var item = docs.Item(i);
                singleResult.Url = item["url"].InnerText;
                singleResult.DisplayUrl = item["domain"].InnerText;
                singleResult.Title = item["title"].InnerText;
                var passages = item["passages"];
                singleResult.Snippet = string.Empty;
                if (passages?.HasChildNodes == true)
                    singleResult.Snippet = passages.FirstChild.InnerText;
                searchResults.Add(singleResult);
            }
            return searchResults;
        }
    }
}
