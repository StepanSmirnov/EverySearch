using EverySearch.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using static System.Web.HttpUtility;
namespace EverySearch.Lib
{
    public class BingProvider : SearchProvider
    {
        private string key;
        private readonly string host = "https://api.cognitive.microsoft.com/bing/v5.0/search";
        private readonly int maxResults = 50;

        public BingProvider(IConfiguration configuration) : base(configuration)
        {
        }

        public override HttpWebRequest MakeRequest(string query, int? count)
        {
            int num = count != null ? Math.Clamp(count.Value, 1, maxResults) : maxResults;
            string encodedQuery = UrlEncode(query);
            string url = $"{host}?q={encodedQuery}&count={num}";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers["Ocp-Apim-Subscription-Key"] = key;
            return request;
        }

        public override IEnumerable<SearchResult> ParseResponse(string result)
        {
            JObject root;
            try
            {
                root  = JObject.Parse(result);
            }
            catch (JsonReaderException e)
            {
                throw new ArgumentException(e.Message);
            }

            if (root.ContainsKey("error"))
            {
                throw new ArgumentException(root["error"]["message"].ToString());
            }

            var items = root["webPages"]["value"].ToArray();
            List<SearchResult> searchResults = new List<SearchResult>();
            foreach (var item in items)
            {
                SearchResult singleResult = new SearchResult();
                singleResult.Title = item["name"].ToString();
                singleResult.Snippet = item["snippet"].ToString();
                singleResult.Url = item["url"].ToString();
                singleResult.DisplayUrl = item["displayUrl"].ToString();
                searchResults.Add(singleResult);
            }
            return searchResults;
        }

        protected override void InitializeCredentials(IConfiguration configuration)
        {
            key = UrlEncode(configuration["Bing:key"]);
        }
    }
}
