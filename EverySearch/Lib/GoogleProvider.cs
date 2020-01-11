using EverySearch.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace EverySearch.Lib
{
    public class GoogleProvider : SearchProvider
    {
        private readonly string host = "https://www.googleapis.com/customsearch/v1";
        private string cx;
        private string key;
        private readonly int maxResults = 10;

        public GoogleProvider(IConfiguration configuration) : base(configuration)
        {
        }

        public override string MakeRequestUrl(string query, int? count)
        {
            int num = count != null ? Math.Clamp(count.Value, 1, maxResults) : maxResults;
            string encodedQuery = System.Web.HttpUtility.UrlPathEncode(query);
            string url = $"{host}?q={encodedQuery}&cx={cx}&key={key}&num={num}";
            return url;
        }

        public override IEnumerable<SearchResult> ParseResult(string result)
        {
            var root = JObject.Parse(result);
            var items = root["items"].ToArray();
            List<SearchResult> searchResults = new List<SearchResult>();
            foreach (var item in items)
            {
                SearchResult singleResult = new SearchResult();
                singleResult.Title = item["htmlTitle"].ToString();
                singleResult.Snippet = item["htmlSnippet"].ToString();
                singleResult.Url = item["link"].ToString();
                singleResult.DisplayUrl = item["displayLink"].ToString();
                searchResults.Add(singleResult);
            }
            return searchResults;
        }

        protected override void InitializeCredentials(IConfiguration configuration)
        {
            key = System.Web.HttpUtility.UrlPathEncode(configuration["Google:key"]);
            cx = System.Web.HttpUtility.UrlPathEncode(configuration["Google:cx"]);
        }
    }
}
